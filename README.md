# Steg-o-saurus

To run: `./bin/debug/net8.0/Stegosaurus`

To Hide: `./bin/debug/net8.0/Stegosaurus -m hide -t text -c "Hello World" -i "./steg.png" -o "./output.png"`
To Hide: `./bin/debug/net8.0/Stegosaurus -m hide -t text -c "This is a test :)" -i "./steg.png" -o "./output.png"`

To Fetch: `./bin/debug/net8.0/Stegosaurus -m fetch -t text -s "./output.png"`

To Show diff:
1. `brew install imagemagick`
2. `magick compare ./steg.png ./output.png ./diff.png`
