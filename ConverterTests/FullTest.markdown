This is a bunch of markdown to test if the converter correctly converts it to RW ready HTML. It can also be used as an example of how to use markdown.

## Headings:

# This is a h1 heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer velit nisi, hendrerit ut lorem quis, condimentum volutpat ligula. Phasellus maximus nisi id accumsan ornare. In interdum ante ut lectus euismod facilisis. Nullam pretium laoreet diam, et suscipit ex mattis sed.

## This is a h2 heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer velit nisi, hendrerit ut lorem quis, condimentum volutpat ligula. Phasellus maximus nisi id accumsan ornare. In interdum ante ut lectus euismod facilisis. Nullam pretium laoreet diam, et suscipit ex mattis sed.

### This is a h3 heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer velit nisi, hendrerit ut lorem quis, condimentum volutpat ligula. Phasellus maximus nisi id accumsan ornare. In interdum ante ut lectus euismod facilisis. Nullam pretium laoreet diam, et suscipit ex mattis sed.

#### This is a h4 heading

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer velit nisi, hendrerit ut lorem quis, condimentum volutpat ligula. Phasellus maximus nisi id accumsan ornare. In interdum ante ut lectus euismod facilisis. Nullam pretium laoreet diam, et suscipit ex mattis sed.

On RW.com, only the h2, h3 and the bold tag you'll see below are supported.

## Formatting tags

This **text** contains several **bold** words. They emphasize **certain** words. This is also called strong emphasis. Use \*\* to create these. 

The *italic* formatting can be good to use in certain, *rare* situations. There are _multiple_ ways of making words _italic_. This is called *emphasis*.

Strikethrough uses two tildes. I love using ~~UE4~~ Unity to make ~~movies~~ games!

Use a \*\*backslash\*\* to escape any \*unwanted\* formatting.

[Links](https://www.google.com) are added by using both [square and round brackets](https://www.raywenderlich.com). If you just add the full URL like https://www.raywenderlich.com that also works.

If you leave add an exclamation sign before the link, it turns into an image instead:

![](Images/doge.jpg)

Lots of formats are supported, including GIFs of course:

![](Images/nigel.gif)

Make sure to add a separate child folder of the folder your markdown is in, you can easily use subfolders in links and images.
The converter can find all locally sourced images, upload them to WordPress and replace the local paths with the image URLs.

## Lists

Lists can be used for making a collection of instructions or items:

- Something blue.
- Something borrowed.
- Something old.

Both - and * can be used to make unordered lists.

* An apple.
* A pear.
* An orange.

Add subitems by adding two spaces or a tab before the line.

* Top level.
* Another top level.
	* Second level
		* Third level.
		* Third level again.
	* Another second level.
* Last top level

To make an ordered (or numbered) list, simply add numbers.

1. Hit the gym.
2. Get lawyered up.
3. ???
4. Profit!

Just make sure to add some text between lists you can get **issues** otherwise.

Here's a divider for fun:

----------

**Don't use the on RW though!**

## Quotes and notes

> This is a simple quote or remark. It uses a > at the start of the line.

Here's an example of a multi-line quote:

> Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer velit nisi, hendrerit ut lorem quis, condimentum volutpat ligula. Phasellus maximus nisi 
> 
> id accumsan ornare. In interdum ante ut lectus euismod facilisis. Nullam pretium laoreet diam, et suscipit ex mattis sed.

If you add **Note:** to a quote, it gets parsed as a note by the converter.

> **Note:** This is a **special** note, it informs you of something important

Another way of adding notes:

:::note
This is also a way of adding notes without having to use "**Note:**" at the start.
You van use both in your markdown without any drawbacks.
:::

Spoilers are unique to RW and don't have an equivalent in markdown. Luckily, you can add these by using pure html (yes, markdown supports HTML as well!):

[spoiler title="Look at the solution"]
Put your spoiler here!
[/spoiler]

The converter can make it easier to add spoilers with some custom logic:

> **Spoiler: Making cake** Put your spoiler here!  
> Another spoiler line.

The converter can also create a spoiler if you create a special kind of quote:

> **Spoiler: This is a spoiler**
> This will be converted to a spoiler.
> 
> You can add multiple lines as well.

You can also add images to quotes and notes:

> **Note:** Look at this image below:
>
> ![](Images/nigel.gif)
>
> Just make sure to add > to all required lines.

## Code

If you surround words with a grave accent (also called a back-tick, unicode 96) they become code. Calling the `OnDestroy()` method for example or adding a `isAwesome` variable. Note that this isn't the same as the ' character (apostrophe, unicode 39).

You can add a block of code by adding three back-ticks, the language of the code and ending with three back-ticks again:

```cs
int i = 1;

switch (i)
{
    case 1:
        Console.WriteLine("One");
        break;
    case 2:
        Console.WriteLine("Two");
        Console.WriteLine("Two");
        break;
    default:
        Console.WriteLine("Other");
        break;
}
```

These are the supported languages:

- **Swift**: swift
- **Objective-C**: objectivec
- **C++**: cpp
- **Bash**: bash
- **JavaScript**: javascript
- **JSON**: json
- **C#**: cs
- **Java**: java
- **Kotlin**: kotlin
- **Gradle**: gradle
- **XML**: xml
- **Shell**: shell
- **Dart**: dart

## Combination test

Lorem **ipsum** *dolor* sit amet, ~~consectetur~~ adipiscing elit. Integer velit nisi, hendrerit ut lorem *quis*, **condimentum** volutpat ligula. 

> [Phasellus](https://www.google.com) maximus nisi id accumsan **ornare**. In interdum ante ut *lectus* euismod facilisis. Nullam pretium laoreet diam, et **suscipit** ex mattis sed.

> **Note:** [Phasellus](https://www.google.com) maximus nisi id accumsan **ornare**. In interdum ante ut *lectus* euismod facilisis. Nullam pretium laoreet diam, et **suscipit** ex mattis sed.

> **Spoiler:Solution inside!**
> Lorem **ipsum** *dolor* sit amet, ~~consectetur~~ adipiscing elit. Integer velit nisi, hendrerit ut lorem *quis*, **condimentum** volutpat ligula. 

## Unity specific tests

**(X:0, Y:0, Z:0)**