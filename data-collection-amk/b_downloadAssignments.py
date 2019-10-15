import boto3 
import collections 
import time 

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

def get_HIT_objects(sandbox = True, client = None):
    # Get all HIT objects associated with your account
    # Docs on what a "HIT object" is: https://docs.aws.amazon.com/AWSMechTurk/latest/AWSMturkAPI/ApiReference_HITDataStructureArticle.html
    if client is None: 
        client = make_conn(sandbox = sandbox )

    # Initial call: 
    callReturn = client.list_hits(MaxResults = 100)
    print(callReturn)
    HIT_list = callReturn['HITs']
    nextToken = callReturn['NextToken']
    # Iterate over tokens
    while(len(callReturn['HITs'])>0): 
        callReturn = client.list_hits(MaxResults = 100, 
            NextToken = nextToken)
        HIT_list.extend(callReturn['HITs'])
        if 'NextToken' in callReturn: 
            nextToken = callReturn['NextToken']
        else: 
            break
    
    return HIT_list

def get_all_HITIds(sandbox = True, client = None): 
    if client is None: 
        client = make_conn(sandbox = sandbox)

    HITobjects = get_HIT_objects(sandbox = sandbox, client = client)

    HITIds = map(lambda h: h['HITId'], HITobjects)
    
    return HITIds

def download_assignments(
    HITIds = None, 
    sandbox = True):

    # Download psychophysics data for the HITIds that are specified. 
    # If HITIds == None, then download data for ALL HITs.

    client = make_conn(sandbox = sandbox )


    if HITIds is None: 
        HITIds = get_all_HITIds(sandbox = sandbox, client = client)
    if type(HITIds) == str: 
        HITIds = [HITIds]


    d = collections.defaultdict(list)

    # Download data
    for i, HITId in enumerate(HITIds): 
        print ('Downloading %d out of %d'%(i+1, len(HITIds)))
        callReturn = client.list_assignments_for_hit(HITId = HITId, 
            MaxResults = 100)
        
        assignments = callReturn['Assignments']
        if 'NextToken' in callReturn: 
            nextToken = callReturn['NextToken']
        else: 
            nextToken = None 

        while(len(assignments)>0): 
            callReturn = client.list_assignments_for_hit(HITId = HITId, 
            MaxResults = 100, 
            NextToken = nextToken)

            assignments.extend(callReturn['Assignments'])

            if 'NextToken' in callReturn: 
                nextToken = callReturn['NextToken']
            else: 
                break 
            
     
        d[HITId].extend(assignments)
    
    # Unpack data
    d_answers = collections.defaultdict(list)
    for hit in d: 
        # Iterate over HITs
        for asn in d[hit]: 
            # Iterate over assignments
            d_answers['assignmentId'].append(asn['AssignmentId'])
            d_answers['HITId'].append(asn['HITId'])
            d_answers['acceptTime'].append(asn['AcceptTime'])
            d_answers['submitTime'].append(asn['SubmitTime'])
            d_answers['assignmentStatus'].append(asn['AssignmentStatus'])
            d_answers['autoApprovalTime'].append(asn['AutoApprovalTime'])
            d_answers['workerId'].append(asn['WorkerId'])
            d_answers['downloadUnixTimestamp'].append(time.time())
            d_answers['answer'].append(asn['Answer'])
            
    return d_answers

  