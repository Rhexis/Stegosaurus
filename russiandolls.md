## Russian Dolls:

Setup:

* `./bin/debug/net8.0/Stegosaurus -m hide -t image -c "./RussianDolls/Russian_Doll_4.png" -i "./RussianDolls/Russian_Doll_3.png" -o "./RussianDolls/Russian_Doll_3_Hide.png"`
* `./bin/debug/net8.0/Stegosaurus -m hide -t image -c "./RussianDolls/Russian_Doll_3_Hide.png" -i "./RussianDolls/Russian_Doll_2.png" -o "./RussianDolls/Russian_Doll_2_Hide.png"`
* `./bin/debug/net8.0/Stegosaurus -m hide -t image -c "./RussianDolls/Russian_Doll_2_Hide.png" -i "./RussianDolls/Russian_Doll.png" -o "./RussianDolls/Russian_Doll_Hide.png"`

Fetch:

1. `./bin/debug/net8.0/Stegosaurus -m fetch -t image -s "./Russian_Doll.png" -o "./Russian_Doll_2.png"`
2. `./bin/debug/net8.0/Stegosaurus -m fetch -t image -s "./Russian_Doll_2.png" -o "./Russian_Doll_3.png"`
3. `./bin/debug/net8.0/Stegosaurus -m fetch -t image -s "./Russian_Doll_3.png" -o "./Russian_Doll_4.png"`

Diff (Compared against the originals):

1. `magick compare ./Russian_Doll_4.png ./RussianDolls/Russian_Doll_4.png ./Russian_Doll_Diff_4.png` (No diff as nothing was hidden)
2. `magick compare ./Russian_Doll_3.png ./RussianDolls/Russian_Doll_3.png ./Russian_Doll_Diff_3.png`
3. `magick compare ./Russian_Doll_2.png ./RussianDolls/Russian_Doll_2.png ./Russian_Doll_Diff_2.png`
4. `magick compare ./Russian_Doll.png ./RussianDolls/Russian_Doll.png ./Russian_Doll_Diff_1.png`