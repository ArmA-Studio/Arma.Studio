private _cfgFunctions = configFile >> "CfgFunctions";
private _preStart = [];
private _preInit = [];
private _postInit = [];
for "_i" from 0 to (count _cfgFunctions - 1) do
{
    private _tagCfg = _cfgFunctions select _i;
    private _tag = configName _tagCfg;
    for "_j" from 0 to (count _tagCfg - 1) do
    {
        private _subCfg = _tagCfg select _j;
        for "_k" from 0 to (count _subCfg - 1) do
        {
            private _cfg = _subCfg select _k;
            private _name = configName _cfg;
            private _path = getText (_cfg >> "file");
            private _content = preprocessFile _path;
            private _code = compile _content;
            missionNamespace setVariable [
                format ["%1_fnc_%2", _tag, _name],
                _code
            ];
            if (getNumber (_cfg >> "PreStart") > 0) then {
                _preStart pushBack _code;
            };
            if (getNumber (_cfg >> "PreInit") > 0) then {
                _preInit pushBack _code;
            };
            if (getNumber (_cfg >> "PostInit") > 0) then {
                _postInit pushBack _code;
            };
        };
    };
};

{ "PreStart" call _x; } forEach _preStart;
{ "PreInit" call _x; } forEach _preInit;
{ "PostInit" call _x; } forEach _postInit;