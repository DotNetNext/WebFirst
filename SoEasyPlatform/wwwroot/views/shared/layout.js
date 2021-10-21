divMenu.$Fill({ SugarUrl: _root + "System/getmenu" })
iframeBody.$AddCss({ "height": $sugar.$WindowHeight() - 75 })
window.onresize = function ()
{
    iframeBody.$AddCss({ "height": $sugar.$WindowHeight() - 75 })
}

 