# Before you get started:

# 0. Set up your AWS account  (https://aws.amazon.com/) and get your aws_access_key_id and aws_secret_access_key
# 1. Install boto3 (http://boto3.readthedocs.io/en/latest/guide/quickstart.html)
# 2. Read the docs for client.create_hit: http://boto3.readthedocs.io/en/latest/reference/services/mturk.html#MTurk.Client.create_hit
# 3. Try using these functions in your own python script!

# boto3 docs forMTurk:
# http://boto3.readthedocs.io/en/latest/reference/services/mturk.html


import boto3
import collections
import os

def make_conn(sandbox = True, verbose = True):
    if sandbox == True:
        print ('Using sandbox')
        endpoint_url = 'https://mturk-requester-sandbox.us-east-1.amazonaws.com'
    else:
        if verbose:
            print ('Press enter to confirm connection to PUBLIC')
            raw_input()
        endpoint_url = 'https://mturk-requester.us-east-1.amazonaws.com'
    conn = boto3.client('mturk', endpoint_url = endpoint_url, region_name='us-east-1')
    return conn


def convert_url_to_question_string(url, frame_height = 768):
    # Converts a url into an XML string in the format MechanicalTurk expects
    external_question_template = lambda url, frame_height: '<ExternalQuestion xmlns="http://mechanicalturk.amazonaws.com/AWSMechanicalTurkDataSchemas/2006-07-14/ExternalQuestion.xsd"><ExternalURL>%s</ExternalURL><FrameHeight>%d</FrameHeight></ExternalQuestion>'%(url, frame_height)

    return external_question_template(url, frame_height)

def upload_html_to_s3(
    html_filepath = None,
    s3_bucket_name = None,
    keyname = None):

    # Uploads an HTML file on your computer to an s3 bucket on your AWS account.
    s3 = boto3.resource('s3')
    s3.meta.client.upload_file(html_filepath, s3_bucket_name, keyname,ExtraArgs= {'ContentType': 'text/html'})

    object_acl = s3.ObjectAcl(s3_bucket_name, keyname)
    response = object_acl.put(ACL='public-read')
    url = os.path.join('https://s3.amazonaws.com', s3_bucket_name, keyname)

    return url

def publish_url_as_HIT(urlList, # List of URL strings
    HIT_group_title = '(Write the title of your task here)',
    HIT_group_description = '(Write your description of the task here)',
    sandbox = True, # Uploading to sandbox or not. Use sandbox for debugging.
    numAssignmentsPerHIT = 2, # How many unique workers for each URL
    durationSecPerHIT = 2700, # How long each assignment will be
    completionRewardUSD = 0.05, # How much you will pay them upon completion.
    ):

    # This function publishes one or more HTML pages (which you have already stored in a public location as a url) as a HIT.
    # You specify how long each assignment is, how many you want, and how much you will be paying for it.

    client = make_conn(sandbox = sandbox)
    print ('\nBalance remaining: ', client.get_account_balance(), '\n')

    # Reasonable defaults for iFrame
    HIT_FRAME_HEIGHT = 768 # pixels
    LifetimeInSeconds = 1209600
    AutoApprovalDelayInSeconds = 86400

    # Create hit_type
    hit_type_response = client.create_hit_type(AutoApprovalDelayInSeconds=AutoApprovalDelayInSeconds, # http://boto3.readthedocs.io/en/latest/reference/services/mturk.html#MTurk.Client.create_hit
                        AssignmentDurationInSeconds=durationSecPerHIT,
                        Reward=str(completionRewardUSD),
                        Title=HIT_group_title,
                        Keywords='MIT vision science game',
                        Description=HIT_group_description,)
    if type(urlList) == str:
        urlList = [urlList]



    d = collections.defaultdict(list)
    HITIds = []
    for url in urlList:
        q = convert_url_to_question_string(url, HIT_FRAME_HEIGHT)
        _hit = client.create_hit_with_hit_type(
            HITTypeId = hit_type_response['HITTypeId'],
            Question = q,
            MaxAssignments=numAssignmentsPerHIT,
            LifetimeInSeconds=LifetimeInSeconds,
            )
        print ('Created HIT %s at %s'%(_hit['HIT']['HITId'], url))
        d['HITId'].append(_hit['HIT']['HITId'])
        d['url'].append(url)
        d['numAssignments'].append(numAssignmentsPerHIT)
        d['assignmentDurationInSeconds'].append(durationSecPerHIT)
        d['completionRewardUSD'].append(completionRewardUSD)
        d['title'].append(HIT_group_title)
        d['sandbox'].append(sandbox)
        d['lifetimeInSeconds'].append(LifetimeInSeconds)
        d['HITFrameHeight'].append(HIT_FRAME_HEIGHT)
        d['autoApprovalDelayInSeconds'].append(AutoApprovalDelayInSeconds)

        worker_requirements = [{
            'QualificationTypeId': '000000000000000000L0',
            'Comparator': 'GreaterThanOrEqualTo',
            'IntegerValues': [99],
            'RequiredToPreview': True,
        }]
        d['QualificationRequirements'] = worker_requirements
        # d['QualificationRequirements']['QualificationTypeId'].append('000000000000000000L0')
        # d['QualificationRequirements']['Comparator'].append('GreaterThan')
        # d['QualificationRequirements']['IntegerValues'].append(98)
        # Qualification in boto3:   https://boto3.amazonaws.com/v1/documentation/api/latest/reference/services/mturk.html
# Qualification Type IDs:   https://docs.aws.amazon.com/AWSMechTurk/latest/AWSMturkAPI-legacy/ApiReference_QualificationRequirementDataStructureArticle.html#ApiReference_QualificationType-IDs
# best practice:   https://blog.mturk.com/qualifications-and-worker-task-quality-best-practices-886f1f4e03fc

    return d


def publish_XML_as_HIT(XMLList, # List of XML strings
    HIT_group_title = '(Write the title of your task here)',
    HIT_group_description = '(Write your description of the task here)',
    sandbox = True, # Uploading to sandbox or not. Use sandbox for debugging.
    numAssignmentsPerHIT = 2, # How many unique workers for each URL
    durationSecPerHIT = 2700, # How long each assignment will be
    completionRewardUSD = 0.05, # How much you will pay them upon completion.
    ):

    # This function publishes one or more XML strings as a HIT.
    # You specify how long each assignment is, how many you want, and how much you will be paying for it.
    # The required format for each XML string can be read about here: https://docs.aws.amazon.com/AWSMechTurk/latest/AWSMturkAPI/ApiReference_QuestionFormDataStructureArticle.html

    client = make_conn(sandbox = sandbox)
    print ('\nBalance remaining: ', client.get_account_balance(), '\n')

    # Reasonable defaults for iFrame
    HIT_FRAME_HEIGHT = 768 # pixels
    LifetimeInSeconds = 1209600
    AutoApprovalDelayInSeconds = 86400

    # Create hit_type
    hit_type_response = client.create_hit_type(AutoApprovalDelayInSeconds=AutoApprovalDelayInSeconds,
                        AssignmentDurationInSeconds=durationSecPerHIT,
                        Reward=str(completionRewardUSD),
                        Title=HIT_group_title,
                        Keywords='MIT vision science game',
                        Description=HIT_group_description,)

    if type(XMLList) is str:
        XMLList = [XMLList]

    d = collections.defaultdict(list)
    HITIds = []
    for xml in XMLList:
        _hit = client.create_hit_with_hit_type( # http://boto3.readthedocs.io/en/latest/reference/services/mturk.html#MTurk.Client.create_hit
            HITTypeId = hit_type_response['HITTypeId'],
            Question = xml,
            MaxAssignments=numAssignmentsPerHIT,
            LifetimeInSeconds=LifetimeInSeconds,
            )
        d['HITId'].append(_hit['HIT']['HITId'])
        d['numAssignments'].append(numAssignmentsPerHIT)
        d['assignmentDurationInSeconds'].append(durationSecPerHIT)
        d['completionRewardUSD'].append(completionRewardUSD)
        d['title'].append(HIT_group_title)
        d['sandbox'].append(sandbox)
        d['lifetimeInSeconds'].append(LifetimeInSeconds)
        d['HITFrameHeight'].append(HIT_FRAME_HEIGHT)
        d['autoApprovalDelayInSeconds'].append(AutoApprovalDelayInSeconds)

    return d
