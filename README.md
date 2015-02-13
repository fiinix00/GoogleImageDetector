# GoogleImageDetector
Detects whats on the image based on chosen file, eg detect what kind of dog showing on an internet image


```csharp
var file = "American_Eskimo_Dog.jpg";

var result = Detector.Detect(file);
```

yields:

    [0]: {[american, 6]}

    [1]: {[eskimo, 7]}

    [2]: {[dog, 5]}

    [3]: {[svenska, 1]}

    [4]: {[kennelklubben, 1]}

    ....
