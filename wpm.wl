protected=Unprotect[Get];
Get[package_/;StringMatchQ[package,"wpm`"~~__]]:=Import ["https://raw.githubusercontent.com/"<>StringRiffle[Insert[Rest[StringSplit[package,"`"]],"master",3],"/"]<>".wl"];
Protect[protected];
