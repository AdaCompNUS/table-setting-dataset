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


### Step 2 - Parse instruction data [TODO]

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



We may amplify the image dataset by generating visual variations through the simulated environment.


---

## Part 4 - Next steps

### Neural network structure
