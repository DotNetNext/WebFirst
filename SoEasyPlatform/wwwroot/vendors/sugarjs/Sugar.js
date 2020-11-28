var $sugar={
  init:function (object) {
        var sugarParameters = {
            Button: object.Button,
            Alert: object.Alert,
            Tip:object.Tip,
            Fill:object.Fill,
            Form:object.Form,
            Grid:object.Grid,
            GridInfo:object.GridInfo,
            Reset:object.Reset,
            Confirm:object.Confirm,
            Ajax:object.Ajax,
            Open:object.Open,
            CloseAll:object.CloseAll,
            Validate:object.Validate,
            FillControls:object.FillControls,
            ClearControls:object.ClearControls,
            SelectTree:object.SelectTree
        };
        HTMLElement.prototype.$Fill = function (value) {
            var element=this;
            if (sugarParameters.Fill == null) {
                   alert("请配置sugar.Fill"); return;
             }
            sugarParameters.Fill(element,value);
        }
       HTMLElement.prototype.$FillControls = function (json) {
            var element=this;
            if (sugarParameters.FillControls == null) {
                   alert("请配置sugar.FillControls"); return;
             }
            sugarParameters.FillControls(element,json);
       }
      HTMLElement.prototype.$ClearControls = function () {
            var element=this;
            if (sugarParameters.ClearControls == null) {
                   alert("请配置sugar.ClearControls"); return;
             }
            sugarParameters.ClearControls(element);
       }
       HTMLElement.prototype.$Form = function (value) {
            var element=this;
            if (sugarParameters.Form == null) {
                   alert("请配置sugar.Form"); return;
            }
            sugarParameters.Form(element,value);
        }
       HTMLElement.prototype.$Grid = function (value) {
            var element=this;
            if (sugarParameters.Grid == null) {
                   alert("请配置sugar.Grid"); return;
            }
            sugarParameters.Grid(element,value);
        }
       HTMLElement.prototype.$SelectTree = function (value) {
            var element=this;
            if (sugarParameters.SelectTree== null) {
                   alert("请配置sugar.SelectTree"); return;
            }
            sugarParameters.SelectTree(element,value);
        }
       HTMLElement.prototype.$GridInfo = function (value) {
            var element=this;
            if (sugarParameters.GridInfo == null) {
                   alert("请配置sugar.GridInfo"); return;
            }
            return sugarParameters.GridInfo(element);
        }
        HTMLElement.prototype.$Button = function (value) {
            var element=this;
            element.addEventListener('click',function(e){
               if (sugarParameters.Button == null) {
                   alert("请配置sugar.Button"); return;
               }
              sugarParameters.Button(element,value);
            })
        }
      HTMLElement.prototype.$Reset = function () {
            var element=this;
            element.addEventListener('click',function(e){
            if (sugarParameters.Reset == null) {
                  alert("请配置sugar.Reset"); return;
            }
            sugarParameters.Reset(element);
           });
        }
        HTMLElement.prototype.$Tip = function (value) {
            var element=this;
            if (sugarParameters.Tip == null) {
                  alert("请配置sugar.Tip"); return;
            }
            sugarParameters.Tip(element,value);
        }
        HTMLElement.prototype.$Open = function (divElement,value) {
            var element=this;
            element.addEventListener('click',function(e){
            if (sugarParameters.Open == null) {
                  alert("请配置sugar.Open"); return;
            }
            return sugarParameters.Open(divElement,value);
            });
        }
        String.prototype.$Alert = function () {
            if (sugarParameters.Alert == null) {
                alert("请配置sugar.Alert"); return;
            }
            sugarParameters.Alert(this);
        }
      HTMLElement.prototype.$Confirm = function (value) {
            var element=this;
            element.addEventListener('click',function(e){
            if (sugarParameters.Confirm == null) {
                  alert("请配置sugar.Confirm"); return;
            }
            sugarParameters.Confirm(value);
            });
        }

      String.prototype.$Ajax = function (value) {
            if (sugarParameters.Ajax == null) {
                alert("请配置sugar.Ajax"); return;
            }
            sugarParameters.Ajax(this,value);
        }
       $sugar.$CloseAll=function(index){
            if (sugarParameters.CloseAll == null) {
                alert("请配置sugar.CloseAll"); return;
            }
            sugarParameters.CloseAll(index);
       }
      $sugar.$Validate=function(json,idPrev){
            if (sugarParameters.CloseAll == null) {
                alert("请配置sugar.Validate"); return;
            }
            sugarParameters.Validate(json,idPrev);
       }
    }
};