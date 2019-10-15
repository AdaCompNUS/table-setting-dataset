# Table setting dataset

> * "Put the dinner fork on the right of the plate"
> * "Put the butter knife on top of the bread plate, pointing towards top right"

If we want robots to assist humans in everyday housework and office work, we need them to be able to understand instructions like the above, which involves spatial descriptions of locations and orientations. To do so, we can build a neural network that predicts the most likely regions to put the target object. It's like the task of grounding spatial reference expressions, only that the network does not locate existing objects but imaged regions in the image.

To build the network, we have created a table-setting dataset that consists of 200 instruction sequences (natural form and linguistic triples form) and corresponding 600 image sequences (which gives us around 7000 before-after image pairs).



## Problem

See a [draft paper](https://github.com/AdaCompNUS/table-setting-dataset/blob/master/paper-draft/main.pdf) of the project's problem statements and plan.



---

## Collect instruction and action data from Amazon Mechanical Turk


### The task of collecting instruction data

We show pairs of before-after images to humans and ask them to write down the instruction in English that describes the action that might have happened between the images, as shown in the screenshot below:

![collect-instructions][https://user-images.githubusercontent.com/18410664/66796219-01f83800-eed5-11e9-81c9-5b1dc57cc8f1.png]


### The task of collecting action data

![collect-actions][https://user-images.githubusercontent.com/18410664/66796218-01f83800-eed5-11e9-9051-c8490b64964e.png]

### The codes for collecting and downloading data

```
pip install virtualenv
virtualenv -p /usr/bin/python2.7 venv   ## create a new virtual environment with python 2.7
source venv/bin/activate    ## enter virtual environment
pip install boto3     ## use functions for Amazon Mechanical Turk (AMT)
pip install jupyter     ## use example jupyter notework for uploading assignments and organizing data
jupyter notebook
```

Then go to `Uploader and Downloader.ipynb`. If the `print(sys.version)` output is 3.*, you can change the Kernel to 2.7 by clicking on the menu bar: Kernel > Change kernel > Python 2.

Here are the sample collected data:

![sample-data-pairs](https://user-images.githubusercontent.com/18410664/66796222-01f83800-eed5-11e9-95b4-1e8c340d0522.png)

Place the dinner knife on the right side of the table cloth

START triples:

```
[you place+1 dinner_knife+652815]
[place+1 on+1 right_side+652817]
[right_side+652817 related-to+2 table_cloth+652816] [related-to+2 is_pp yes]
```

Innerese (Semantic structure):
```
PLACE ( (you)
        (
            OBJECT (
                (dinner_knife))
            ON (
            OF ( (right_side)
                (table_cloth)))))
```
---



## Synthesize image data in Unity

We may amplify the image dataset by generating visual variations through the simulated environment.

## Next steps

### network structure
