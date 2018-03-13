
# ForgottenPasswordGenerator
*This description is under writing*

Straightforward brute force attack is almost never successful when you know nothing about the password that is long enough. Fortunately you usually remember some parts of the password and you need a convinient tool that fill the gaps. This software is that tool.

To generate passwords you need to define a password pattern. The pattern consists of sections. A section is a subpattern, a part of password with own parameters of generating of combinations. You define different sections according what you know about the password.

#### Example 1
You remember that the password begins with word "love" in arbitrary case and 4 digits behind. You can split the password into two sections:
1. A "fixed section" of word "love" with the option of arbitrary case for each character.
2. An "arbitrary section" with minmumum and maximum lengths equal 4 consists of digits 0-9.

This pattern gives you only 160,000 combinations! It is very few compare to dozens of millions and billions arbitrary combinations.

There are two types of sections:
1. Fixed section.
2. Arbitrary section.

### Fixed section
You should use this section when you know a sequence of characters. You may not remember its case or even its length. The position of each character is fixed. Because of such strict rule that section generates very few combinations.

### Arbitrary section
You should use this section when you know just a character set and possible length range. This section potentially emits a huge number of combinations. Avoid using of this section with big length & character set.

Along with sections you can set other password-wide parameters that can decrease the total number of combinations:
1. **MaxSingeCharSequenceLength** allows you to limit repeating characters. For example, you know exactly that password does not contain three or more characters at row like "aa", "aaa" or "00", "000". This parameters is usefull when the password contains words. There are no words that contain triple letters (same letter three times in a row).

*To be continue...*
