Unprotect[Get];
	Get[package_/;StringMatchQ[package,"wpm`"~~__]]:=If[Not@TrueQ[Evaluate@Symbol[package<>"loaded"]],
Import ["https://raw.githubusercontent.com/"<>StringRiffle[Insert[Rest[StringSplit[package,"`"]],"master",3],"/"]<>".wl"];
Evaluate[Symbol[package<>"loaded"]]:=True;]
Protect[Get];
