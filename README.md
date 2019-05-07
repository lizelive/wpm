# wpm
A simple but effective package manager for wolfram language.
# Useage
To install run (you need to do this in each file you use wpm)

```mathematica
Import["https://wpm.lize.live"];
```
(not, as of writeing https does not work yet)

To use it, just import the package like your normally do.

```mathematica
>>`wpm`lizelive`lwl`EmConst`
```
This example loads the package at https://github.com/lizelive/lwl/blob/master/EmConst.wl for an example.
See the entire https://github.com/lizelive/lwl repo for an example of how to make a package.

# adding a package
Add your source code to a github. The package name should be

```mathematica
`wpm`username`repo`pathInRepo`
```

# converntions
Please use the wolfram standard for metadata.
add multiload = true to allow loading your package multiple time. This is not typically a good idea.
You can have multiple packages per repo, but that's bad practice and may be depricated in the future.
