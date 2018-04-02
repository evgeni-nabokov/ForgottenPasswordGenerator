# ForgottenPasswordGenerator
*This description is under writing*

Straightforward brute force attack is almost never successful when you know nothing about the password that is long enough. Fortunately you usually remember some parts of the password and you need a convinient tool that fill the gaps. This software is that tool.

*A permutation is an arrangement of objects (with or without repetition), where the order does matter.
A combination is selections of objects, with or without repetition, where order does not matter
A variation is an arrangement of selections of objects, where the order of the selected objects does matter.*

## Variation Generators
To generate passwords you need to use one or more variation generators.
There are a few generators:
1. **Fixed**. A fixed string. Case is optional. 
2. **Arbitrary**. A set of any string of specified symbols within specified length range. Case is optional.
3. **Number Range**. A set of numbers within a range. A step is optional.
4. **String List**. A set of specified strings.
5. **Compound**. An ordered sequence of generators.

#### Example 1
You remember that the password begins with the word "love" with unknown case for each character and 4 digits behind. You can combine two generators:
1. A fixed generator with the word "love" with the option of both cases for each character.
2. A arbitrary generator of "0123456789" symbols with minmumum and maximum lengths equal to 4.

This pattern gives you only 160,000 variations! It is a very few compare to dozens of millions and billions arbitrary variations.

### Fixed generator
You should use this generator when you know a sequence of particular symbols. You may not remember its case or even its length. The position of each symbol is fixed. Because of such strict rule that generator generates not many variations.

### Arbitrary generator
You should use this generator when you know just a symbol set and possible length range, but you do not know the order of symbols and exact length. This generator is the most common and potentially able to emit a huge number of variations. Avoid using of this generation with big length & symbol set.

### Number Range generator
You should use this generator when you need to generate integer numbers within a range using a specified step. You can also define a format string. 


### String List generator
You should use this generator when you need to generate variations from your specified set. 

### Compound generator
You should use this generator to combine different generatos together. The order does matter.

## Suppressors
Along with generators you can define a set of suppressor that can radically decrease the total number of variations:
1. **Adjacent Duplicates**. Limits repeating symbols. For example, you know exactly that password does not contain three or more symbols at row like "aaa" or "aaaa". This suppressor is usefull when the password contains words. There are no words that contain triple letters (same letter three times in a row).
2. **Adjacent Same Case**. Limits repeating same case charactes (upper or lower). This parameters is usefull when the password contains words some of them you typed with a capital character.
3. **Duplicates Spacing**
4. **Same Case Spacing**
5. **Regex**

 
*To be continue...*
