# Table setting dataset

> * "Put the dinner fork on the right of the plate"
> * "Put the butter knife on top of the bread plate, pointing towards top right"

If we want robots to assist humans in everyday housework and office work, we need them to be able to understand instructions like the above, which involves spatial descriptions of locations and orientations. To do so, we can build a neural network that predicts the most likely regions to put the target object. It's like the task of grounding spatial reference expressions, only that the network does not locate existing objects but imaged regions in the image.

To build the network, we have created a table-setting dataset that consists of 200 instruction sequences (natural form and linguistic triples form) and corresponding 600 image sequences (which gives us around 7000 before-after image pairs).



## Part 1 - Problem

See a [draft paper](https://github.com/AdaCompNUS/table-setting-dataset/blob/master/paper-draft/main.pdf) of the project's problem statements and plan.



## Part 2 - Collect instruction and action data from Amazon Mechanical Turk


### Summary of the table-setting dataset

* 339 instruction sequences
* 902 image sequences

Here is a sample of collected image sequence with the corresponding instructions:

![sample-data-pairs](https://user-images.githubusercontent.com/18410664/66796222-01f83800-eed5-11e9-95b4-1e8c340d0522.png)

40 example sequences of instructions, original and parsed, can be seen [here](https://nusrls.s3.amazonaws.com/triplesNTrees_40.html).

692 example instruction-image pairs can be seen [here](https://nusrls.s3.amazonaws.com/table-setting_reconstructed_images_692.html).



### Step 1 - Collect instruction data

We show pairs of before-after images to humans and ask them to write down the instruction in English that describes the action that might have happened between the images, as shown in the screenshot below:

![collect-instructions](https://user-images.githubusercontent.com/18410664/66796219-01f83800-eed5-11e9-81c9-5b1dc57cc8f1.png)

The image pairs are screenshots from five YouTube videos about table-setting:

* Video 2: [How to set the table - Anna Post](https://www.youtube.com/watch?v=KoU1XiQJ1vo)
* Video 3: [How to Set a Table](https://www.youtube.com/watch?v=LktSuL8__7M)
* Video 4: [Learn How to Set a Formal Dinner Table](https://www.youtube.com/watch?v=p9mzBckf3G4)
* Video 5: [How To Dine At A Proper Place Setting](https://www.youtube.com/watch?v=6SoS4UPSSx8)

You can perform the task yourself [here](https://nusrls.s3.amazonaws.com/HIT_give_instructions.html).

The codes for collecting and downloading data are detailed in directory `data-collection-amk/``


### Step 2 - Parse instruction data [TODO: add descriptions of the parsers]

Here is an example instruction:

> Place the dinner knife on the right side of the table cloth

Here are the START triples for it:

```
[you place+1 dinner_knife+652815]
[place+1 on+1 right_side+652817]
[right_side+652817 related-to+2 table_cloth+652816]
[related-to+2 is_pp yes]
```

Here is the Innerese structure (a kind of semantic structure):
```
PLACE ( (you)
        (
            OBJECT (
                (dinner_knife))
            ON (
            OF ( (right_side)
                (table_cloth)))))
```



### Step 3 - Collect action data

We show images of a table and common cutleries used in table-setting. Then given a sequence of instructions, the human will drag and rotate the images to carry out the instruction, as shown in the image below:

![collect-actions](https://user-images.githubusercontent.com/18410664/66796218-01f83800-eed5-11e9-9051-c8490b64964e.png)

The instruction sequences are selected from the data collected from the first set of workers. The instructions selected are meant to be clear and precise.

You can perform the task yourself [here](https://nusrls.s3.amazonaws.com/HIT_set_table.html).




### Step 4 - Synthesize image data in Unity

To use the simulation world for generating image data, follow the below steps:

* Install Unity and create a user name
* Create a new Unity project named "Table Setting World"
* Download the ["Table Setting Project" package](https://www.dropbox.com/s/9bnja2ynh4e7hql/Table%20Seeting%20Project.unitypackage?dl=0) and import into your "Table Setting World"
* Download the [data]() for the project and unzip it in the root folder of the project


#### The image dataset

In the unzipped data folder, you will see:

* `all collected actions/`: there are two batches of collected human actions in csv files
* `all generated images/`: there are correspondingly two batches of generated images, with name in the format of "HIT_***_config_***_*.png"
* `all related instructions`: the files that relate the images to instructions. For each worker submission, there is:
  * one json file of the instructions presented to the worker
  * one image file of the collage of all images generated based on his actions
  * one image file of the final table set by the worker
* `locations/`: the initial configurations of the table, 10 for each video gives 50 different configurations in total
* `input/`: to put the human actions csv files
* `output/`: where new data will be generated


#### The script for generating image

In the "Table Setting Project" package, we mainly use the script `MakePictures.cs`. It synthesizes the sequences of images according to the recorded actions of human workers who used the drag-and-rotate web interface

The script takes in all the csv files in "data/input/", reconfigure the table according to each csv file, and take a scrrenshot for each configuration of the table.

Example input file name: HIT_34_configs_102_4.csv

* It is the 34 submission by human workers
* It starts with table configuration 102, which is the second configuration of video 1
* It is the fifth (start with index 0) scene of the action sequence
* Each row of the csv file corresponds to the location of one
* The corresponding output file name is HIT_34_configs_102_4.png

The input files are generated using the Jupyter notebook `Uploader and Downloader.ipynb` (the last part of the notebook)

P.S. We may amplify the image dataset by generating visual variations in the simulated environment (lighting; the shape, texture, and materials of objects).


#### The script for generating collage

To better visualize the image and instruction data, there is a script for combining all images in one action sequence into a collage and creating an HTML page to display it along with the instructions presented to the worker as well as the final table set by the worker.

That script for creating collage (another jupyter notebook) is in folder `data-collection-amk/image_data_collage/`


---

## Part 4 - Next steps

### Neural network structure
