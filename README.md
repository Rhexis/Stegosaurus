# Steg-o-saurus

To run: `./bin/debug/net8.0/Stegosaurus`

To Hide:

* `./bin/debug/net8.0/Stegosaurus -m hide -t text -c "Hello World" -i "./steg.png" -o "./output.png"`
* `./bin/debug/net8.0/Stegosaurus -m hide -t text -c "This is a test :)" -i "./steg.png" -o "./output.png"`
* `./bin/debug/net8.0/Stegosaurus -m hide -t image -c "./me.png" -i "./steg.png" -o "./output.png"`

To Fetch:

* `./bin/debug/net8.0/Stegosaurus -m fetch -t text -s "./output.png"`
* `./bin/debug/net8.0/Stegosaurus -m fetch -t image -s "./output.png"`

To Show diff:

1. `brew install imagemagick` (Or visit https://imagemagick.org/script/download.php)
2. `magick compare ./steg.png ./output.png ./diff.png`


## Russian Dolls:

Setup:

* `./bin/debug/net8.0/Stegosaurus -m hide -t image -c "./RussianDolls/Russian_Doll_4.png" -i "./RussianDolls/Russian_Doll_3.png" -o "./RussianDolls/Russian_Doll_3_Hide.png"`
* `./bin/debug/net8.0/Stegosaurus -m hide -t image -c "./RussianDolls/Russian_Doll_3_Hide.png" -i "./RussianDolls/Russian_Doll_2.png" -o "./RussianDolls/Russian_Doll_2_Hide.png"`
* `./bin/debug/net8.0/Stegosaurus -m hide -t image -c "./RussianDolls/Russian_Doll_2_Hide.png" -i "./RussianDolls/Russian_Doll.png" -o "./RussianDolls/Russian_Doll_Hide.png"`

Fetch:

* `./bin/debug/net8.0/Stegosaurus -m fetch -t image -s "./Russian_Doll.png" -o "./Russian_Doll_2.png"`
* `./bin/debug/net8.0/Stegosaurus -m fetch -t image -s "./Russian_Doll_2.png" -o "./Russian_Doll_3.png"`
* `./bin/debug/net8.0/Stegosaurus -m fetch -t image -s "./Russian_Doll_3.png" -o "./Russian_Doll_4.png"`

Diff (Compared against the originals):

1. `magick compare ./Russian_Doll_4.png ./RussianDolls/Russian_Doll_4.png ./Russian_Doll_Diff_4.png` (No diff as nothing was hidden)
2. `magick compare ./Russian_Doll_3.png ./RussianDolls/Russian_Doll_3.png ./Russian_Doll_Diff_3.png`
3. `magick compare ./Russian_Doll_2.png ./RussianDolls/Russian_Doll_2.png ./Russian_Doll_Diff_2.png`
4. `magick compare ./Russian_Doll.png ./RussianDolls/Russian_Doll.png ./Russian_Doll_Diff_1.png`