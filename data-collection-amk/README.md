```
pip install virtualenv
virtualenv -p /usr/bin/python2.7 venv   ## create a new virtual environment with python 2.7
source venv/bin/activate    ## enter virtual environment
pip install boto3     ## use functions for Amazon Mechanical Turk (AMT)
pip install jupyter     ## use example jupyter notework for uploading assignments and organizing data
jupyter notebook
```

Then go to `Uploader and Downloader.ipynb`. If the `print(sys.version)` output is 3.*, you can change the Kernel to 2.7 by clicking on the menu bar: Kernel > Change kernel > Python 2.





# show-and-tell-data-collector
A general frame work for collecting instructions and demonstrations data through Amazon Mechanical Turk

## Step 1 - Configure AWS

### [Create S3 Bucket]


### [Install AWS Command Line Interface (CLI)](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-install.html)
```
pip install awscli --upgrade --user
aws --version
```

### [Configure AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/cli-chap-configure.html)
```
aws configure
```

## instruction selected list

table1: 15, 17, 18, 111,    114, 185, 188, 236, 237
table2: 8, 12, 58, 83,      182, 187, 191, 245
table3: 11, 30, 43, 56,     67, 79, 200, 235
table4: 16, 28, 124, 131,   146, 157, 229, 249
table5: 7, 22, 29, 31,      57, 74, 143, 218

coffee cup over
308 225
505 218
306 30

406 249 nowhere to put the bread plate

charger on plate first

408 249 moved nothing

objects shouldn't overlap
can't put off the table cloth
remind humans to go previous
