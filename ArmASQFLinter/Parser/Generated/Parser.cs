using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using System;
using NLog;


namespace RealVirtuality.SQF.Parser
{
    public class Parser {
    	public const int _EOF = 0;
	public const int _T_SCALAR = 1;
	public const int _T_HEX = 2;
	public const int _T_STRING = 3;
	public const int _T_STRINGTABLESTRING = 4;
	public const int _T_IDENT = 5;
	public const int maxT = 39;

        const bool _T = true;
        const bool _x = false;
        const int minErrDist = 2;

        public Scanner scanner;
        public Errors  errors;

        public Token t;    // last recognized token
        public Token la;   // lookahead token
        int errDist = minErrDist;
    public readonly string[] _BINARY_OPERATORS = {"waypointattachobject","enableattack","removeaction","addplayerscores","neartargets","setmarkersizelocal","campreparefov","isflatempty","ctrlsetfontheightsecondary","ctrlsetfonth5b","ctrlsettextcolor","lbcolorright","lnbdeletecolumn","tvsetpictureright","setobjecttextureglobal","setobjectmaterialglobal","hideselection","setforcegeneratorrtd","removecuratoreditableobjects","swimindepth","setsize","addgoggles","setmimic","additem","disableconversation","setfsmvariable","inareaarray","ctrlsetstructuredtext","setcuratorwaypointcost","setcuratoreditingareatype","menusetvalue","setlightintensity","attachobject","fademusic","getrelpos","removeweaponturret","action","fire","setvehicleammodef","newoverlay","backpackspacefor","kbreact","setfatigue","lnbsetcurselrow","setgusts","configclasses","animatesource","setdynamicsimulationdistancemult","setsimpletasktarget","setsimpletasktype","exec","lockwp","setparticlecircle","campreparerelpos","findemptyposition","ctrlsettextsecondary","lnbsetcolumnspos","lnbpicture","tvsetcursel","animationphase","setactualcollectivertd","setcuratorcoef","menuurl","ctrlsetmodelscale","addvehicle","removemenuitem","setobjectarguments","getvariable","assignasdriver","lnbdata","buttonsetaction","settargetage","then","ordergetin","setmarkersize","cuttext","faderadio","campreparedive","hcgroupparams","forceweaponfire","kbhastopic","lnbsettext","addteammember","foreachmemberteam","menusetdata","setlightnings","enabledynamicsimulation","setsimpletaskalwaysvisible","addlivestats","updatedrawicon","enableaifeature","triggerattachobject","setface","setrank","forceadduniform","getdir","addmenuitem","setfriend","createdisplay","lbsetpicturecolor","menutext","sendtask","engineon","addweaponglobal","linkitem","synchronizewaypoint","inrangeofartillery","addownedmine","step","addheadgear","setgroupowner","drawicon","lbadd","lbsettextright","cbsetchecked","revealmine","setunitloadout","nearroads","moveincargo","domove","setnamesound","inserteditorobject","setvehicletipars","ctrlcommit","lbpicture","lnbaddcolumn","lnbsetpicturecolorselectedright","enableautostartuprtd","setpilotcameratarget","ppeffectcommit","do","settriggeractivation","setfog","setwaypointtimeout","setgroupidglobal","lockturret","lbcolor","lbsetpicturecolorselected","enablepersonturret","addcuratoreditingarea","menudelete","setmagazineturretammo","enableaimprecision","getsoundcontroller","setconvoyseparation","canvehiclecargo","switchlight","camsetfocus","exitwith","cutfadeout","setdamage","setovercast","camsetdive","worldtomodel","assignascargoindex","buildingpos","from","createmenu","canadditemtouniform","sethitindex","lockedturret","fadespeech","displayctrl","lbsettext","lbpictureright","setbleedingremaining","enableuavwaypoints","setmarkershapelocal","setwaypointdescription","switchcamera","listobjects","addrating","campreparefocus","nearobjectsready","removealleventhandlers","gethidefrom","allow3dmode","enablefatigue","lbisselected","lnbsetpicturecolorselected","tvsetcolor","customradio","vectordiff","drawlocation","addmagazineturret","setwaypointvisible","setdropinterval","getfriend","setpos","magazineturretammo","setmarkershape","seteditorobjectscope","land","setmarkertextlocal","setlightambient","nearentities","isequalto","tvsetselectcolor","tvsort","addcuratoreditableobjects","setwinddir","animationsourcephase","removesimpletask","spawn","campreparefovrange","setvectorup","countside","campreparedir","limitspeed","setposatl","getobjectargument","setdestination","setunitability","kbtell","ctrlsettooltipcolortext","lnbsortbyvalue","removecuratoraddons","menusetpicture","setoxygenremaining","ctrlcreate","showneweditorobject","ondoubleclick","distancesqr","addbackpackglobal","iskindof","editobject","intersect","setvehiclearmor","nmenuitems","hideobject","kbwassaid","ctrlmapscreentoworld","set3denlayer","unregistertask","!=","joinassilent","setvectordirandup","removehandgunitem","setflagside","setwaypointtype","emptypositions","setwaypointname","remoteexec","foreachmember","clear3denattribute","get3denmissionattribute","setwindstr","vectoradd","settaskstate","deleteat","enablesimulation","objstatus","weapondirection","removemagazineturret","forcespeed","moveindriver","additemtouniform","removeallmpeventhandlers","gethit","drawarrow","lbsetpicturerightcolordisabled","tvsetdata","tvsetpicturerightcolordisabled","ctrlremovealleventhandlers","setunloadincombat","setrectangular","isequaltype","additemtovest","params","addmenu","setuseractiontext","setvehicleposition","assignascargo","sideradio","moveincommander","create3denentity","pushback","isflashlighton","setposworld","createvehiclelocal","triggerattachvehicle","inarea","setmarkerpos","assignitem","setdrawicon","setmarkeralphalocal","landat","disablecollisionwith","ctrlsetfontp","ctrlsetautoscrolldelay","setskill","savestatus","getenvsoundcontroller","diarysubjectexists","removeweaponattachmentcargo","dofire","allowdamage","setwaypointscript","setspeaker","setrepaircargo","setvehicleid","ctrlsetfont","lbsetvalue","lnbsetpicturecolorright","displayaddeventhandler","setsuppression","setleader","vectormultiply","removeweaponcargo","getobjectchildren","drawpolygon","closedisplay","skillfinal","tvsetpicturecolordisabled","append","addbackpackcargoglobal","apply","stop","say2d","countunknown","select","addmagazinecargo","enablegunlights","ctrlsettooltip","lbtext","lbsetpicturerightcolor","lnbsetpicturecolor","curatorcoef","setparticleparams","deleteeditorobject","cameraeffect","addmagazinecargoglobal","getcargoindex","doartilleryfire","showlegend","catch","setunitpos","setcamuseti","htmlload","lnbsetvalue","setcustomweightrtd","selectdiarysubject","ppeffectadjust","selectionposition","setmarkeralpha","additemcargoglobal","preloadobject","setpitch","setmusiceffect","drawrectangle","lnbsetcolorright","menuenable","menupicture","menuvalue","menusort","allowgetin","foreach","commandartilleryfire","setmarkerbrush","<=","canadd","loadmagazine","ctrlshow","execvm","lbsetselectcolor","addcuratorcameraarea","menushortcut","setautonomous","setunittrait","canslingload","registertask","settaskresult","distance2d","enablereload","setunconscious","nearobjects","removeitem","countenemy","isequaltypearray","turretunit","setlightflaresize","tvsettooltip","enablemimics","ctrlsetmodeldirandup","creatediarysubject","unlinkitem","execfsm","call","selectweapon","setvehiclelock","setflagtexture","addprimaryweaponitem","switchmove","commandtarget","ctrlsetfonth2b","addcuratoraddons","currentmagazineturret","currentmagazinedetailturret","minedetectedby","setcustomaimcoef","setvehiclecargo","onshownewobject","removeweaponglobal","getreldir","setgroupid","setvehicleammo","camsetpos",">=","createvehicle","debugfsm","attachto","drawline","lnbvalue","tvsetpicture","progresssetposition","ctrlremoveeventhandler","set3denmissionattribute","animatedoor","setwaypointloitertype","setwaypointstatements","respawnvehicle","flyinheight","setidentity","settitleeffect","isuniformallowed","joinsilent","addmagazineammocargo","removesecondaryweaponitem","switchgesture","camcommit","setvelocitytransformation",">>","kbaddtopic","ctrlenable","displayseteventhandler","lnbtext","connectterminaltouav","setshotparents","ppeffectenable","adduniform","allowdammage","setcollisionlight","ctrlmapcursor","enableirlasers","enablecopilot","ctrlsetfonth4b","tvsortbyvalue","loadidentity","getunittrait","createtask","settext","setside","setbehaviour","^","loadoverlay","assignasgunner","isequaltypeparams","removeitemfromvest","playmove","addbackpackcargo","hcremovegroup","setweaponreloadingtime","posscreentoworld","tvsetpicturecolor","set3denattribute","isuavconnectable","setfaceanimation","counttype","setwaypointposition","&&","groupchat","globalchat","lbtextright","allowsprint","to","useaudiotimeformoves","fadesound","setrandomlip","modeltoworldvisual","setrain","updatemenuitem","setformdir","createunit","removedrawlinks","commandchat","cutrsc","groupselectunit","allowcrewinimmobile","ctrlsetfonth6b","lnbsetpicture","tvdata","tvvalue","setwingforcescalertd","allowcuratorlogicignoreareas","menuenabled","doorphase","vectorcrossproduct","ctrlsetmodel","geteditorobjectscope","drawlink","addmagazineglobal","setmarkercolorlocal","joinstring","addsecondaryweaponitem","getartilleryeta","gethitindex","lockdriver","lnbaddrow","lnbtextright","lnbsettextright","controlsgroupctrl","lock","sort","camsetfovrange","globalradio","setmarkertext","enableai","addscore","playaction","addwaypoint","arrayintersect","camcreate","unitsbelowheight","weaponsturret","ctrlsetbackgroundcolor","lbsetselected","radiochannelremove","setlightdaylight","synchronizeobjectsremove","addbackpack","commandmove","setattributes","commandfollow","seteditormode","ammo","pushbackunique","lightattachobject","skill","tvpictureright","ctrlmapanimadd","ctrlmapworldtoscreen","synchronizeobjectsadd","ropeattachto","say","resize","%","setwaypointcompletionradius","findnearestenemy","setunitposweak","removemagazines","playgesture","dosuppressivefire","splitstring","add3denlayer","setcenterofmass","setsimpletaskdescription","setdir","camsettarget","set","setvectordir","moveto","getobjectproxy","count","evalobjectargument","addpublicvariableeventhandler","setgroupiconparams","setgroupicon","enablecollisionwith","setunitrecoilcoefficient","tvtooltip","lbsetpictureright","createmissiondisplay","setposition","setmarkertypelocal","removemagazineglobal","groupradio","setfuelcargo","addmpeventhandler","setdammage","hintc","setcaptive","settriggerstatements","findcover","setcombatmode","hcsetgroup","disabletiequipment","loadstatus","menusize","vectordistance","setstamina","setdynamicsimulationdistance","removedrawicon","weaponaccessoriescargo","assignteam","camsetfov","actionparams","setspeedmode","settriggertext","setradiomsg","setformation","removempeventhandler","joinas","lbsetcursel","tvpicture","displayremoveeventhandler","customchat","currentweaponturret","inpolygon","canadditemtobackpack","setwaypointbehaviour","allowfileoperations","seteffectcondition","setpilotlight","lnbpictureright","addcuratorpoints","setdebriefingtext","getsoundcontrollerresult","sendtaskresult","createsimpletask","*","setvisibleiftreecollapsed","assignasturret","editorseteventhandler","ctrlsetfontpb","lbsetcolor","tvsettext","setcuratorcameraareaceiling","setlightattenuation","creatediaryrecord","setimportance","setvariable","dowatch","setvehiclevarname","moveobjecttoend","campreparetarget","param","canadditemtovest","+","onmapsingleclick","getpos","setlightcolor","getgroupicon","hideobjectglobal","lbsetpicturecolordisabled","lnbsetdata","lnbcolorright","tvcollapse","foreachmemberagent","setbrakesrtd","vectorfromto","enablechannel","setspeech","lockcamerato","atan2","setposasl","setwppos","tofixed","sendsimplecommand","moveingunner","deleterange","ctrlsetfontsecondary","ctrlsettext","ctrlsetfonth1","tvexpand","nearestobject","get3denattribute","menudata","setlightflaremaxdistance","setmass","removemagazinesturret","settaskmarkeroffset","addeventhandler","campreload","-","setmarkerdir","isirlaseron","setwaypointcombatmode","knowsabout","ctrlsetfonth2","ctrlsetactivecolor","lnbdeleterow","lnbsetcolor","vectordistancesqr","worldtomodelvisual","isequaltypeall","removeitems","setwaypointformation","lockcargo","forcewalk","lookat","setowner","kbadddatabasetargets","ctrlsetfonth3","ctrlsettooltipcolorshade","commandfsm","lbsetpicturerightcolorselected","slidersetspeed","ppeffectforceinnvg","setcamerainterest","modeltoworld","setmarkerposlocal","addmagazines","playmovenow","in","assignascommander","countfriendly","setmarkercolor","throw","addmagazine","dotarget","/","sidechat","addvest","reveal","setcurrentwaypoint","addhandgunitem","aimedattarget","ctrlsetfonth4","lnbsort","ctrladdeventhandler","radiochannelsetlabel","remoteexeccall","deleteresources","menushortcuttext","setwaves","setslingload","commandfire","setname","addweaponturret","createsite","addweaponcargo","getfsmvariable","disableai","setwindforce","drawellipse","ctrlsetfonth5","lbsetdata","tvsetpicturerightcolor","saveidentity","enablestamina","setpilotcamerarotation","settype","try","camcommand","additemtobackpack","weaponaccessories","setparticlefire","getspeed","setammo","removemagazine","sethit","lockedcargo","ctrlsetfonth6","ctrlsettooltipcolorbox","setwaypointloiterradius","deletevehiclecrew","turretlocal","addweaponitem","checkvisibility","setmarkerbrushlocal","removeweapon","removegroupicon","mapcenteroncamera","ctrlsetangle","lbvalue","lnbcolor","tvsetpicturerightcolorselected","slidersetposition","slidersetrange","radiochannelsetcallsign","addresources","menusetaction","setlightuseflare","addweapon","campreparepos","setformationtask","setpipeffect","inflame","camconstuctionsetparams","switchaction","setairportside","publicvariableclient","ctrlsetchecked","assigncurator","menuaction","menuchecked","lbsettooltip","setanimspeedcoef","allowfleeing","suppressfor","addeditorobject","flyinheightasl","showwaypoint","commandradio","vehicleradio","findeditorobject","setflagowner","posworldtoscreen","ctrlsetposition","tvadd","tvsetvalue","callextension","creatempcampaigndisplay","vectorcos","setdirection","setwaypointhouseposition","campreparebank","setmarkertype","addscoreside","move","camcommitprepared","setvelocity","disablenvgequipment","ctrlsetfade","lbsetpicture","lbsetcolorright","moveinany","setpilotcameradirection","ropedetach","setcurrenttask","waypointattachvehicle","sethidebehind","max","targetknowledge","commandwatch","show3dicons","camsetdir","setsoundeffect","setobjectproxy","setwaypointforcebehaviour","lnbsetpictureright","tvdelete","tvsetpicturecolorselected","setfromeditor","setrotorbrakertd","set3denobjecttype","distance","mod","say3d","cutobj","moveinturret","removeitemfromuniform","setlightbrightness","directsay","buildingexit","synchronizetrigger","hasweapon","fireattarget","kbremovetopic","menuadd","menucollapse","join","dofollow","setposasl2","min","setparticlerandom","enablesimulationglobal","execeditorscript","playactionnow","magazinesturret","ctrlsetfontheight","ctrlsetfontheighth1","ctrlsetautoscrollrewind","ctrlseteventhandler","ctrlsetdisabledcolor","animate","setobjecttexture","setdriveonpath","targetsaggregate","removeitemfrombackpack","breakout","gethitpointdamage","assigntoairport","ctrlsetfonth1b","ctrlsetfontheighth2","setwantedrpmrtd","removecuratorcameraarea","forcefollowroad","setsimpletaskdestination","||","find","commandsuppressivefire","settriggertype","camsetbank","setunitrank","and",":","==","hcselectgroup","findemptypositionready","ctrlsetfontheighth3","ctrlsetautoscrollspeed","tvtext","enableautotrimrtd","setrainbow","selectweaponturret","disableuavconnectability","enableropeattach","targetsquery","leavevehicle","lookatpos","setmarkerdirlocal","removeprimaryweaponitem","servercommand","unassignitem","setammocargo","kbadddatabase","ctrlsetfontheighth4","ctrlsetscale","ctrlsettextcolorsecondary","lbsetselectcolorright","setenginerpmrtd","enableuavconnectability","enablevehiclecargo","addaction","addweaponcargoglobal","settriggerarea","removeownedmine","or","<","addgroupicon","glanceat","ctrlsetfonth3b","ctrlsetfontheighth5","lbdata","tvcount","removecuratoreditingarea","triggerdynamicsimulation","selectleader","additemcargo","camsetrelpos","nearsupplies","setfuel","vehiclechat","remotecontrol","ctrlsetfontheighth6","dofsm","lbdelete","removeteammember","menusetcheck","setsimpletaskcustomdata",">","removeeventhandler","setwaypointspeed","setparticleclass","selecteditorobject","copywaypoints","settriggertimeout","else","isequaltypeany","sethitpointdamage","ctrlsetforegroundcolor","displayremovealleventhandlers","radiochanneladd","setobjectmaterial","menuexpand","setposaslw","vectordotproduct","turretowner"};
	public readonly string[] _UNARY_OPERATORS = {"showradio","vectorup","lnbsetcurselrow","animationnames","currentzeroing","isonroad","triggerarea","get3dengrid","settrafficspeed","supportinfo","menucollapse","taskdescription","isautohoveron","ctrlangle","menusetdata","moonphase","onplayerdisconnected","isshowing3dicons","lbsetcolor","getallhitpointsdamage","lbsetcolorright","triggertext","getfatigue","ctrlshown","eyedirection","ctrlmodel","tvtext","ropeunwind","fillweaponsfrompool","hcselected","asltoagl","isobjectrtd","enabletraffic","roledescription","simpletasks","getammocargo","flagowner","deletegroup","isclass","tvdata","getenginetargetrpmrtd","waypointattachedvehicle","speed","menusetvalue","reloadenabled","rating","hideobject","useaiopermapobstructiontest","primaryweapon","addtoremainscollector","uisleep","ctrlmodelscale","curatorcameraareaceiling","removevest","gearslotdata","formationposition","gettrimoffsetrtd","getmissionconfigvalue","simulcloudocclusion","movetofailed","playmission","rankid","showgps","synchronizedtriggers","removeallitemswithmagazines","setaperturenew","simulinclouds","menuurl","isarray","leaderboardsrequestuploadscorekeepbest","removebackpackglobal","exportjipmessages","ctrlhtmlloaded","getmarkercolor","markeralpha","face","lbsetselectcolorright","getvehiclecargo","deletelocation","ceil","waypointvisible","enablesatnormalondetail","getposatl","objectcurators","activatekey","disableremotesensors","attackenabled","curatoreditingarea","playmusic","playmusic","assignedteam","vectordirvisual","lnbsetvalue","triggeractivated","set3denmissionattributes","unassignvehicle","boundingbox","ppeffectcreate","movetime","waypointspeed","tvpictureright","if","ctrltext","ctrltext","ctrlclassname","actionname","animationstate","markertype","menuchecked","lineintersectsobjs","weaponcargo","buttonaction","buttonaction","set3deniconsvisible","hcshowbar","drop","isturnedout","addweaponpool","camtarget","setwinddir","reverse","getcameraviewdirection","set3denattributes","slidersetrange","textlogformat","deletemarkerlocal","lnbsettext","exp","waypointstatements","scudstate","sliderrange","sliderrange","isautotrimonrtd","boundingboxreal","terrainintersect","ropedestroy","tvsetpictureright","createteam","floor","lnbsettextright","param","lbsetpicturecolordisabled","setcamshakeparams","lognetwork","abs","debriefingtext","lnbsortbyvalue","istouchingground","taskmarkeroffset","aisfinishheal","tvsetpicture","vest","headgear","lnbsetdata","fuel","agltoasl","weaponsitemscargo","linearconversion","removemissioneventhandler","damage","getmodelinfo","str","tostring","dogetout","getbleedingremaining","squadparams","groupfromnetid","leader","leader","leader","settrafficdistance","currentthrowable","enableengineartillery","terrainintersectasl","debuglog","lnbsetpicturecolorselectedright","lnbaddarray","savevar","onbriefingteamswitch","lbsetvalue","edit3denmissionattributes","uniformitems","getcustomaimcoef","ingameuiseteventhandler","leaderboardrequestrowsfriends","showcommandingmenu","unitrecoilcoefficient","unassigncurator","actionkeysnames","objectparent","clearmagazinecargo","hostmission","canmove","getstatvalue","tvsettext","positioncameratoworld","getwingsorientationrtd","isstaminaenabled","weaponsitems","vectornormalized","unitbackpack","finite","lnbtext","teamname","pickweaponpool","surfaceiswater","getslingload","reload","tvsetdata","allvariables","allvariables","allvariables","allvariables","allvariables","allvariables","allvariables","setterraingrid","speaker","lbsettooltip","weapons","selectededitorobjects","removeall3deneventhandlers","leaderboardsrequestuploadscore","lnbdata","unitisuav","lbadd","lnbclear","lnbclear","assignedtarget","cameraeffectenablehud","execfsm","attachedto","showuavfeed","querymagazinepool","effectivecommander","sizeof","landresult","cbchecked","onhcgroupselectionchanged","ctrlscale","asin","lbtext","clearweaponcargoglobal","assigneddriver","allcontrols","taskresult","lbpicture","do3denaction","numbertodate","leaderboardstate","tvsetpicturecolor","secondaryweaponmagazine","setaperture","showpad","ctrlidc","getdir","uniformcontainer","lbsortbyvalue","isplayer","buldozer_enableroaddiag","campreloaded","local","local","drawicon3d","surfacetype","lbdata","lbdelete","boundingcenter","enablediaglegend","ctrlidd","fleeing","getgroupiconparams","cutobj","iscopilotenabled","uniform","delete3denentities","commandgetout","wfsidetext","wfsidetext","wfsidetext","tvexpandall","tvexpandall","velocitymodelspace","getallownedmines","onpreloadstarted","modparams","flagtexture","getfuelcargo","groupid","rotorsrpmrtd","waypointloiterradius","size","captivenum","waypointtimeout","tvsort","selectionnames","lbselection","position","position","canunloadincombat","attachedobjects","netid","netid","waypointcompletionradius","removeallassigneditems","ropeunwound","waypointposition","isdlcavailable","waypointtype","addswitchableunit","closeoverlay","tvexpand","getartilleryammo","tvadd","owner","progressloadingscreen","estimatedtimeleft","driver","displayparent","sleep","create3denentity","primaryweaponmagazine","ctrlparentcontrolsgroup","actionkeysimages","enablesentences","sin","while","try","curatoreditableobjects","entities","entities","tvcount","setgroupiconsselectable","showwarrant","assigneditems","copytoclipboard","!","groupselectedunits","titlefadeout","atltoasl","loaduniform","someammo","toarray","setwind","groupowner","isweaponrested","isaimprecisionenabled","isagent","commander","markerpos","leaderboardinit","taskcustomdata","lnbdeletecolumn","incapacitatedstate","saveoverlay","magazinesallturrets","collectivertd","tvdelete","format","taskcompleted","playsound3d","getanimaimprecision","execvm","waypointformation","cantriggerdynamicsimulation","get3denconnections","ongroupiconoverenter","backpackmagazines","cutrsc","comment","weaponlowered","handshit","selectbestplaces","removeallcuratorcameraareas","taskparent","triggertype","hidebody","getpilotcameratarget","menuenabled","showchat","case","scoreside","behaviour","getmagazinecargo","nearestterrainobjects","lifestate","issprintallowed","classname","difficultyoption","getfieldmanualstartpage","islocalized","triggertimeout","remoteexec","units","units","removeallcontainers","hcleader","systemchat","detectedmines","getobjecttype","ropeattachedto","diag_codeperformance","inputaction","oncommandmodechanged","movetocompleted","requiredversion","lnbaddrow","+","+","textlog","openyoutubevideo","combatmode","ppeffectdestroy","ppeffectdestroy","add3deneventhandler","canstand","vectormagnitude","rotorsforcesrtd","count","count","count","-","formationleader","radiochannelcreate","enginestorquertd","isengineon","add3denconnection","for","for","collapseobjecttree","waypointhouseposition","getplayerscores","enableradio","scriptdone","skill","settimemultiplier","waypointtimeoutcurrent","magazinecargo","ropecut","creatediarylink","backpack","ctrlmapanimcommit","mapanimadd","surfacenormal","lineintersectswith","hcremoveallgroups","waituntil","getposworld","toupper","showwatch","configsourcemodlist","createtrigger","getstamina","waypointshow","ctrltype","getmass","weaponstate","lbpictureright","load","loadabs","removeswitchableunit","inheritsfrom","lnbsort","islighton","simulationenabled","currentmagazinedetail","onmapsingleclick","screenshot","unitaimpositionvisual","actionids","everybackpack","asltoatl","sethudmovementlevels","set3denmodelsvisible","currentmuzzle","ctrlautoscrollspeed","currentweaponmode","getwingspositionrtd","waypointloitertype","name","name","onbriefinggroup","locationposition","importance","captive","isweapondeployed","menushortcut","assert","keyimage","removeallweapons","titleobj","lbsort","lbsort","vehiclevarname","triggertimeoutcurrent","ctrlmodeldirandup","assignedgunner","setmouseposition","terminate","soldiermagazines","getmarkerpos","endmission","leaderboardrequestrowsglobal","magazinesammo","removeuniform","faction","ctrltextsecondary","clear3deninventory","lnbpictureright","servercommandavailable","geteditormode","removeallprimaryweaponitems","menuhover","menuhover","verifysignature","group","allturrets","allturrets","restarteditorcamera","camcommitted","tvtooltip","startloadingscreen","ctrlsettext","enabledynamicsimulation","currenttask","flagside","isinremainscollector","nearestobject","magazinesammocargo","setplayable","unlockachievement","lbsettext","isautonomous","additempool","getmissionlayerentities","lnbdeleterow","throw","dissolveteam","publicvariableserver","handgunmagazine","getoxygenremaining","progressposition","tvvalue","vehicle","buldozer_loadnewroads","removeallactions","vectormagnitudesqr","getnumber","servercommand","attachedobject","everycontainer","ctrlautoscrollrewind","stopenginertd","preprocessfilelinenumbers","gunner","lbsetdata","isvehiclecargo","agent","openmap","openmap","playsound","playsound","dostop","oneachframe","lightdetachobject","getpersonuseddlcs","getgroupicons","getwppos","setsimulweatherlayers","getdescription","ropeendposition","text","text","items","showcinemaborder","ctrlautoscrolldelay","onpreloadfinished","nearestlocation","getrepaircargo","titlersc","setcurrentchannel","lbsetpictureright","menusize","pitch","onbriefingplan","formattext","camerainterest","removeheadgear","side","side","side","completedfsm","playableslotsnumber","servercommandexecutable","enablesaving","remove3denconnection","queryitemspool","isformationleader","numberofenginesrtd","velocity","setplayerrespawntime","preloadsound","getallsoundcontrollers","setviewdistance","rad","getmarkertype","magazinesdetail","markersize","ctrlshow","waypointattachedobject","ishidden","preloadtitleobj","tvcollapseall","tvcollapseall","ctrlparent","diag_dynamicsimulationend","registeredtasks","deg","forceatpositionrtd","actionkeysnamesarray","titlecut","configsourceaddonlist","menuaction","disableuserinput","set3denlinesvisible","aimpos","cancelsimpletaskdestination","clearweaponcargo","enableenvironment","markercolor","createsimpleobject","airportside","assignedvehiclerole","lnbgetcolumnsposition","lnbgetcolumnsposition","enableteamswitch","waypointforcebehaviour","precision","ropes","lbcolor","settrafficdensity","call","createsoundsource","backpackcontainer","vectorupvisual","istext","leaderboardrequestrowsglobalarounduser","vectordir","clearbackpackcargoglobal","getdlcusagetime","members","worldtoscreen","teamtype","removeallhandgunitems","private","curatoreditingareatype","magazinesdetailbackpack","with","ctrlmapscale","createdialog","currentwaypoint","createmarker","magazinesammofull","hint","putweaponpool","getobjectdlc","enabledebriefingstats","goto","deletewaypoint","setshadowdistance","tvsortbyvalue","lockidentity","typeof","score","lbsetpicturecolor","nextmenuitemindex","lasertarget","unitready","showmap","deletemarker","isautostartupenabledrtd","allmissionobjects","getcenterofmass","stance","curatorpoints","lbtextright","alive","getterrainheightasl","crew","triggerattachedvehicle","rank","getrotorbrakertd","itemswithmagazines","selectmax","selectmin","isbleeding","isrealtime","ctrlactivate","acos","processdiarylink","lbcolorright","menupicture","namesound","isnil","locked","ctrlenable","getdlcassetsusagebyname","clearbackpackcargo","cuttext","formationdirection","creategroup","preloadtitlersc","getweaponcargo","isabletobreathe","getassignedcuratorunit","ctrlcommitted","get3denentity","ctrlmapanimdone","setgroupiconsvisible","gearslotammocount","enginespowerrtd","markerbrush","sethorizonparallaxcoef","echo","dynamicsimulationenabled","dynamicsimulationenabled","hcallgroups","setcamshakedefparams","diag_log","screentoworld","default","menuenable","currentcommand","sliderposition","sliderposition","unitpos","finddisplay","itemcargo","secondaryweaponitems","menuvalue","createmarkerlocal","round","deleteidentity","getaimingcoef","breakout","ropeattachedobjects","lbsetselectcolor","mineactive","enablestressdamage","mapcenteroncamera","lbsetpicture","handgunweapon","activateaddons","addmagazinepool","synchronizedwaypoints","synchronizedwaypoints","vehiclecargoenabled","save3deninventory","compilefinal","cos","taskhint","moveout","setstatvalue","deleteteam","get3denactionstate","createagent","importallgroups","assignedvehicle","expecteddestination","goggles","removeallcuratoraddons","random","random","objectfromnetid","ctrlchecked","tvsettooltip","isburning","getobjectmaterials","getplayeruid","binocular","getweaponsway","collect3denhistory","handgunitems","removeallcuratoreditingareas","skiptime","getpos","getpos","curatorcameraarea","enableaudiofeature","publicvariable","create3dencomposition","showscoretable","backpackcargo","getobjecttextures","vestmagazines","curatoraddons","secondaryweapon","atan","get3denlayerentities","nearestobjects","getposvisual","deletecollection","triggerstatements","switch","priority","menusetpicture","setlocalwindparams","lbvalue","createcenter","menuexpand","getposaslw","atg","lineintersectssurfaces","ongroupiconoverleave","camdestroy","curatorwaypointcost","slidersetposition","ismarkedforcollection","getassignedcuratorlogic","triggeractivation","ctrldelete","parsetext","teammember","actionkeys","waypointbehaviour","preloadcamera","parsenumber","parsenumber","deletecenter","remoteexeccall","setstaminascheme","rectangular","move3dencamera","addmusiceventhandler","resetsubgroupdirection","lognetworkterminate","channelenabled","lnbsetpictureright","cleargroupicons","taskdestination","taskalwaysvisible","vestitems","log","switchcamera","ppeffectcommitted","ppeffectcommitted","buttonsetaction","menudelete","fromeditor","waypointsenableduav","tvcollapse","lnbcolorright","airdensityrtd","keyname","closedialog","lnbsetcolumnspos","commandstop","scriptname","list","hintc","tan","detach","needreload","waypointdescription","image","dynamicsimulationdistancemult","tvsetcursel","tvclear","tvclear","isnull","isnull","isnull","isnull","isnull","isnull","isnull","isnull","isnull","setacctime","remove3denlayer","removebackpack","hideobjectglobal","lnbcolor","lnbcurselrow","lnbcurselrow","getbackpackcargo","typename","getshotparents","curatorregisteredobjects","ctrlenabled","ctrlenabled","removeallmusiceventhandlers","playersnumber","onplayerconnected","menutext","lnbaddcolumn","menushortcuttext","mapgridposition","ropeattachenabled","firstbackpack","lnbsetpicture","getitemcargo","tvsetvalue","removemusiceventhandler","gettext","getburningvalue","formation","formation","simulclouddensity","tolower","localize","loadbackpack","unassignteam","removeallownedmines","menudata","ropelength","getanimspeedcoef","fullcrew","fullcrew","createvehicle","formationmembers","addcamshake","markertext","getcontainermaxload","type","type","lnbsetpicturecolor","visibleposition","getpilotcameradirection","speedmode","vestcontainer","currenttasks","markerdir","showhud","showhud","getposasl","showcuratorcompass","forcemap","inflamed","loadfile","waypoints","scopename","menusetaction","weaponinertia","haspilotcamera","isforcedwalk","currentvisionmode","enablecaustics","selectrandom","deactivatekey","setdate","direction","direction","dynamicsimulationdistance","ppeffectenabled","getdlcs","getsuppression","lbsetpicturecolorselected","settrafficgap","camusenvg","resources","getarray","hintsilent","assignedcommander","taskchildren","updateobjecttree","showcompass","isnumber","lnbpicture","waypointname","deletesite","nearestbuilding","nearestbuilding","addforcegeneratorrtd","forcegeneratorrtd","getposatlvisual","getallenvsoundcontrollers","clearitemcargoglobal","getmissionconfig","sqrt","ctrltextheight","removegoggles","showsubtitles","getplayerchannel","menuadd","clearitemcargo","sendudpmessage","checkaifeature","uavcontrol","iswalking","compile","tasktype","flag","composetext","formleader","stopped","clearallitemsfrombackpack","leaderboardgetrows","nearestlocations","addmissioneventhandler","ongroupiconclick","hmd","setobjectviewdistance","setobjectviewdistance","remove3deneventhandler","waypointscript","lnbtextright","magazinesdetailuniform","tvsetpicturerightcolor","hintcadet","set3dengrid","roadsconnectedto","lbcursel","lbcursel","canfire","creategeardialog","slidersetspeed","configproperties","sendaumessage","iskeyactive","isobjecthidden","configsourcemod","lnbvalue","tvpicture","underwater","showwaypoints","sliderspeed","sliderspeed","confighierarchy","setmusiceventhandler","unitaimposition","lnbsize","lnbsize","ropecreate","deletestatus","lnbsetpicturecolorselected","morale","ctrlfade","selectplayer","menusetcheck","createlocation","menuclear","menuclear","failmission","lnbsetcolorright","preprocessfile","setcompassoscillation","iscollisionlighton","removeallmissioneventhandlers","ctrlvisible","formationtask","ismanualfire","getpilotcamerarotation","opendlcpage","ln","wingsforcesrtd","setdefaultcamera","removefromremainscollector","playscriptedmission","lnbsetcolor","deletevehicle","onteamswitch","lineintersects","isuavconnected","ctrlposition","lbsize","lbsize","getunitloadout","getunitloadout","getunitloadout","roadat","createguardedpoint","commitoverlay","params","currentweapon","getdirvisual","ctrlmapmouseover","drawline3d","ctrlmapanimclear","leaderboarddeinit","clearoverlay","enginesrpmrtd","datetonumber","setsystemofunits","breakto","difficultyenabled","clearmagazinecargoglobal","synchronizedobjects","tg","useaisteeringcomponent","getdammage","lbsetcursel","setarmorypoints","weightrtd","loadvest","gearidcammocount","nearestlocationwithdubbing","createmine","lnbsetpicturecolorright","getmarkersize","getposaslvisual","eyepos","removeallitems","createvehiclecrew","uniformmagazines","enablecamshake","tvcursel","tvcursel","setdetailmapblendpars","onbriefingnotes","markershape","backpackitems","magazines","waypointcombatmode","enginesisonrtd","queryweaponpool","didjipowner","primaryweaponitems","visiblepositionasl","set3denselected","get3denselected","lightison","assignedcargo","currentmagazine","taskstate","magazinesdetailvest","not","lbclear","lbclear","getpilotcameraposition","forcerespawn","titletext","getconnecteduav","geteditorcamera","configname","menusort","get3denentityid","lockeddriver","ctrlsetfocus","unitaddons"};
	public readonly string[] _NULL_OPERATORS = {"sideenemy","safezonex","clearmagazinepool","pixelh","slingloadassistantshown","windstr","isinstructorfigureenabled","safezoney","getclientstatenumber","getclientstate","date","getelevationoffset","current3denoperation","getterraingrid","player","exit","mapanimclear","halt","missiondifficulty","nextweatherchange","allsites","teamswitchenabled","blufor","visiblescoretable","is3denmultiplayer","worldname","ismultiplayersolo","isremoteexecuted","curatorcamera","displaynull","resistance","diag_frameno","currentnamespace","armorypoints","pixelgrid","configfile","controlnull","markasfinishedonsteam","visiblewatch","estimatedendservertime","diag_activemissionfsms","hudmovementlevels","sidelogic","opfor","rainbow","cansuspend","servertime","teams","all3denentities","librarycredits","shownwarrant","allmines","getshadowdistance","alldisplays","isautotest","viewdistance","sideambientlife","allmapmarkers","worldsize","rain","didjip","missionstart","airdensitycurvertd","sunormoon","false","safezonewabs","profilename","difficultyenabledrtd","savegame","hasinterface","clearitempool","runinitscript","getartillerycomputersettings","visiblemap","forceweatherchange","shownchat","mapanimdone","resetcamshake","showncompass","winddir","pixelgridbase","currentchannel","ispipenabled","gettotaldlcusagetime","get3denlinesvisible","sidefriendly","endloadingscreen","profilenamesteam","playableunits","cheatsenabled","opencuratorinterface","gusts","missionconfigfile","overcastforecast","commandingmenu","distributionregion","windrtd","allcurators","benchmark","getmissiondlcs","istuthintsenabled","shownartillerycomputer","scriptnull","isfilepatchingenabled","missionname","nil","clearweaponpool","true","issteammission","linebreak","language","diag_fps","acctime","enableenddialog","safezoneh","allunitsuav","shownradio","allplayers","clearforcesrtd","clearradio","disabledebriefingstats","logentities","pixelw","visiblecompass","locationnull","shownwatch","playerside","lightnings","simulweathersync","shownmap","shownpad","independent","campaignconfigfile","clientowner","getobjectviewdistance","overcast","safezonexabs","forceend","diag_activesqfscripts","objnull","selectnoplayer","getdlcassetsusage","diag_activesqsscripts","wind","servername","freelook","sideempty","humidity","hcshownbar","diag_ticktime","visiblegps","pi","shownuavfeed","ismultiplayer","productversion","missionnamespace","isdedicated","pixelgridnouiscale","alldeadmen","diag_fpsmin","showncuratorcompass","saveprofilenamespace","finishmissioninit","fog","buldozer_isenabledroaddiag","userinputdisabled","netobjnull","loadgame","forcedmap","cameraview","moonintensity","dynamicsimulationenabled","isserver","reversedmousey","librarydisclaimers","getremotesensorsdisabled","tasknull","get3dencamera","profilenamespace","cadetmode","cameraon","allgroups","agents","briefingname","shownscoretable","copyfromclipboard","cursorobject","east","teamswitch","diag_activescripts","isstressdamageenabled","fogparams","time","shownhud","switchableunits","playerrespawntime","groupiconsvisible","initambientlife","allcutlayers","curatormouseover","get3denmouseover","civilian","missionversion","activatedaddons","showngps","cursortarget","sideunknown","difficulty","fogforecast","savejoysticks","buldozer_reloadopermap","getmissionlayers","is3den","dialog","systemofunits","radiovolume","savingenabled","particlesquality","west","mapanimcommit","vehicles","curatorselected","parsingnamespace","musicvolume","waves","disableserialization","daytime","grpnull","teammembernull","soundvolume","alldead","getresolution","timemultiplier","safezonew","allunits","getmouseposition","groupiconselectable","uinamespace","get3deniconsvisible","isstreamfriendlyuienabled","confignull"};
	
	public readonly string _ERR_NO_BINARY_OPERATOR = "Expected binary operator.";
	public readonly string _ERR_NO_UNARY_OPERATOR = "Expected unary operator.";
	public readonly string _ERR_NO_NULL_OPERATOR = "Expected null operator.";
	
	//ToDo: Fix -<EXPRESSION> not being detected correctly
	
    

        public Parser(Scanner scanner) {
            this.scanner = scanner;
            errors = new Errors();
        }
        
        bool peekCompare(params int[] values)
        {
            Token t = la;
            foreach(int i in values)
            {
                if(i != -1 && t.kind != i)
                {
                    scanner.ResetPeek();
                    return false;
                }
                if (t.next == null)
                    t = scanner.Peek();
                else
                    t = t.next;
            }
            scanner.ResetPeek();
            return true;
        }
        bool peekString(int count, params string[] values)
        {
            Token t = la;
            for(; count > 0; count --)
                t = scanner.Peek();
            foreach(var it in values)
            {
                if(t.val == it)
                {
                    scanner.ResetPeek();
                    return true;
                }
            }
            scanner.ResetPeek();
            return false;
        }
        
        
        void SynErr (int n) {
            if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n, t.charPos, t == null ? 0 : t.val.Length);
            errDist = 0;
        }
        void Warning (string msg) {
            errors.Warning(la.line, la.col, msg);
        }

        public void SemErr (string msg) {
            if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg, t.charPos, t == null ? 0 : t.val.Length);
            errDist = 0;
        }
        public void SemErr (int line, int col, string msg, int charPos, int length) {
            if (errDist >= minErrDist) errors.SemErr(line, col, msg, charPos, length);
            errDist = 0;
        }
        
        void Get () {
            for (;;) {
                t = la;
                la = scanner.Scan();
                if (la.kind <= maxT) { ++errDist; break; }
    
                la = t;
            }
        }
        
        void Expect (int n) {
            if (la.kind==n) Get(); else { SynErr(n); }
        }
        
        bool StartOf (int s) {
            return set[s, la.kind];
        }
        
        void ExpectWeak (int n, int follow) {
            if (la.kind == n) Get();
            else {
                SynErr(n);
                while (!StartOf(follow)) Get();
            }
        }


        bool WeakSeparator(int n, int syFol, int repFol) {
            int kind = la.kind;
            if (kind == n) {Get(); return true;}
            else if (StartOf(repFol)) {return false;}
            else {
                SynErr(n);
                while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
                    Get();
                    kind = la.kind;
                }
                return StartOf(syFol);
            }
        }

        
    	void SQFDOCUMENT() {
		STATEMENT();
		while (la.kind == 8) {
			SEMICOLON();
			if (StartOf(1)) {
				STATEMENT();
			}
		}
	}

	void STATEMENT() {
		if (_T_IDENT == la.kind && peekString(1, "=") || peekString(1, "private") ) {
			ASSIGNMENT();
		} else if (peekString(0, "private") && peekString(1, "[") ) {
			EXPRESSION();
		} else if (StartOf(1)) {
			EXPRESSION();
		} else SynErr(40);
	}

	void SEMICOLON() {
		Expect(8);
		while (la.kind == 8) {
			Get();
		}
	}

	void EXP_CODE() {
		Expect(6);
		if (StartOf(1)) {
			STATEMENT();
			while (la.kind == 8) {
				SEMICOLON();
				if (StartOf(1)) {
					STATEMENT();
				}
			}
		}
		Expect(7);
	}

	void ASSIGNMENT() {
		if (la.kind == 9) {
			Get();
		}
		Expect(5);
		Expect(10);
		EXPRESSION();
	}

	void EXPRESSION() {
		EXP_OR();
	}

	void EXP_OR() {
		EXP_AND();
		if (la.kind == 11 || la.kind == 12) {
			if (la.kind == 11) {
				Get();
			} else {
				Get();
			}
			EXP_OR();
		}
	}

	void EXP_AND() {
		EXP_COMPARISON();
		if (la.kind == 13 || la.kind == 14) {
			if (la.kind == 13) {
				Get();
			} else {
				Get();
			}
			EXP_AND();
		}
	}

	void EXP_COMPARISON() {
		EXP_BINARY();
		if (StartOf(2)) {
			switch (la.kind) {
			case 15: {
				Get();
				break;
			}
			case 16: {
				Get();
				break;
			}
			case 17: {
				Get();
				break;
			}
			case 18: {
				Get();
				break;
			}
			case 19: {
				Get();
				break;
			}
			case 20: {
				Get();
				break;
			}
			case 21: {
				Get();
				break;
			}
			}
			EXP_COMPARISON();
		}
	}

	void EXP_BINARY() {
		EXP_ELSE();
		if (la.kind == 5) {
			Get();
			if(!_BINARY_OPERATORS.Contains(t.val.ToLower())) SemErr(_ERR_NO_BINARY_OPERATOR); 
			EXP_BINARY();
		}
	}

	void EXP_ELSE() {
		EXP_ADDITION();
		if (la.kind == 22) {
			Get();
			EXP_ELSE();
		}
	}

	void EXP_ADDITION() {
		EXP_MULTIPLICATION();
		if (StartOf(3)) {
			if (la.kind == 23) {
				Get();
			} else if (la.kind == 24) {
				Get();
			} else if (la.kind == 25) {
				Get();
			} else {
				Get();
			}
			EXP_ADDITION();
		}
	}

	void EXP_MULTIPLICATION() {
		EXP_POWER();
		if (StartOf(4)) {
			if (la.kind == 27) {
				Get();
			} else if (la.kind == 28) {
				Get();
			} else if (la.kind == 29) {
				Get();
			} else if (la.kind == 30) {
				Get();
			} else {
				Get();
			}
			EXP_MULTIPLICATION();
		}
	}

	void EXP_POWER() {
		EXP_HIGHEST();
		if (la.kind == 32) {
			Get();
			EXP_POWER();
		}
	}

	void EXP_HIGHEST() {
		if (la.kind == 5 || la.kind == 9 || la.kind == 35) {
			EXP_UNARYNULL();
		} else if (la.kind == 1 || la.kind == 2 || la.kind == 3) {
			EXP_VALUES();
		} else if (la.kind == 33) {
			Get();
			EXPRESSION();
			Expect(34);
		} else if (la.kind == 6) {
			EXP_CODE();
		} else if (la.kind == 36) {
			EXP_ARRAY();
		} else SynErr(41);
	}

	void EXP_UNARYNULL() {
		var tmpToken = la; 
		if (la.kind == 5) {
			Get();
		} else if (la.kind == 9) {
			Get();
		} else if (la.kind == 35) {
			Get();
		} else SynErr(42);
		if (StartOf(1)) {
			if(la.kind == _T_IDENT && _BINARY_OPERATORS.Contains(la.val.ToLower()) && !_UNARY_OPERATORS.Contains(la.val.ToLower())) return; 
			EXP_HIGHEST();
			if(tmpToken.kind == _T_IDENT && !_UNARY_OPERATORS.Contains(tmpToken.val.ToLower())) SemErr(tmpToken.line, tmpToken.col, _ERR_NO_UNARY_OPERATOR, tmpToken.charPos, tmpToken.val.Length); 
		}
	}

	void EXP_VALUES() {
		if (la.kind == 1) {
			Get();
		} else if (la.kind == 2) {
			Get();
		} else if (la.kind == 3) {
			Get();
		} else SynErr(43);
	}

	void EXP_ARRAY() {
		Expect(36);
		if (StartOf(1)) {
			EXPRESSION();
			while (la.kind == 37) {
				Get();
				EXPRESSION();
			}
		}
		Expect(38);
	}


    
        public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
    		SQFDOCUMENT();
		Expect(0);

        }
        
        static readonly bool[,] set = {
    		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_T,_T,_T, _x,_T,_T,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_T, _T,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x}

        };
    } // end Parser


    public class Errors {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public int Count { get { return this.ErrorList.Count; } }
        public List<Tuple<int, int, string>> ErrorList;
        public string errMsgFormat = "line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
        public Errors()
        {
            ErrorList = new List<Tuple<int, int, string>>();
        }

        public virtual void SynErr (int line, int col, int n, int offset, int length) {
            string s;
            switch (n) {
    			case 0: s = "EOF expected"; break;
			case 1: s = "T_SCALAR expected"; break;
			case 2: s = "T_HEX expected"; break;
			case 3: s = "T_STRING expected"; break;
			case 4: s = "T_STRINGTABLESTRING expected"; break;
			case 5: s = "T_IDENT expected"; break;
			case 6: s = "\"{\" expected"; break;
			case 7: s = "\"}\" expected"; break;
			case 8: s = "\";\" expected"; break;
			case 9: s = "\"private\" expected"; break;
			case 10: s = "\"=\" expected"; break;
			case 11: s = "\"||\" expected"; break;
			case 12: s = "\"or\" expected"; break;
			case 13: s = "\"&&\" expected"; break;
			case 14: s = "\"and\" expected"; break;
			case 15: s = "\"==\" expected"; break;
			case 16: s = "\"!=\" expected"; break;
			case 17: s = "\">\" expected"; break;
			case 18: s = "\"<\" expected"; break;
			case 19: s = "\">=\" expected"; break;
			case 20: s = "\"<=\" expected"; break;
			case 21: s = "\">>\" expected"; break;
			case 22: s = "\"else\" expected"; break;
			case 23: s = "\"+\" expected"; break;
			case 24: s = "\"-\" expected"; break;
			case 25: s = "\"max\" expected"; break;
			case 26: s = "\"min\" expected"; break;
			case 27: s = "\"*\" expected"; break;
			case 28: s = "\"/\" expected"; break;
			case 29: s = "\"%\" expected"; break;
			case 30: s = "\"mod\" expected"; break;
			case 31: s = "\"atan2\" expected"; break;
			case 32: s = "\"^\" expected"; break;
			case 33: s = "\"(\" expected"; break;
			case 34: s = "\")\" expected"; break;
			case 35: s = "\"!\" expected"; break;
			case 36: s = "\"[\" expected"; break;
			case 37: s = "\",\" expected"; break;
			case 38: s = "\"]\" expected"; break;
			case 39: s = "??? expected"; break;
			case 40: s = "invalid STATEMENT"; break;
			case 41: s = "invalid EXP_HIGHEST"; break;
			case 42: s = "invalid EXP_UNARYNULL"; break;
			case 43: s = "invalid EXP_VALUES"; break;

                default: s = "error " + n; break;
            }
            logger.Error(string.Format(errMsgFormat, line, col, s));
            ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
		}

        public virtual void SemErr (int line, int col, string s, int offset, int length) {
            logger.Error(string.Format(errMsgFormat, line, col, s));
            ErrorList.Add(new Tuple<int, int, string>(offset, length, s));
		}
        
        public virtual void SemErr (string s) {
            logger.Error(s);
        }
        
        public virtual void Warning (int line, int col, string s) {
            logger.Warn(string.Format(errMsgFormat, line, col, s));
        }
        
        public virtual void Warning(string s) {
            logger.Warn(s);
        }
    }


    public class FatalError: Exception {
        public FatalError(string m): base(m) {}
    }
}
