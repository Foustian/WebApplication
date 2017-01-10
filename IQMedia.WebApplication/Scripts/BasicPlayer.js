var _PreviousPlayState = 0;
var _PlayState = 0;
var _Start = 0;
var _Stop = 0;
var _Width = 545;
var _Vol = 0;
var _SeekSecond = 0;
var _ID = null;
var _firstCall = false;
var _currenttime = 0;
var _currentTimeInt = 0;
var _processedTitle120 = -1;
var _programTitle = "NA";
var _nielsenData = null;
var _categoryData = null;
var _clipID = null;
var _flash = null;
var _agentGuid = null;
var videotype = '';
var xIndex = 0;
var LastIndex = 0;
var _CurrentTotalPlaySeconds = 1;
var _IsCheckSettings = true;
var _ForceCategorySelection = false;
var _TotalTime = 0;

_flash = document.getElementById('HUY');

if (_flash != null) {
    _flash.style.outline = "medium none";
    AddPlayerEvtListners();
}