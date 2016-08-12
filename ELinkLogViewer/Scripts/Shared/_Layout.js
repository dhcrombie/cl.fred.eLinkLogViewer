(function () {
    $(document).ready(function () {
        ko.applyBindings(new vmStatus(), $(".eLink-Status")[0]);
    });

    function vmStatus() {
        var self = this;
        self.koServiceStatus = ko.observable({
            Contacted: false,
            Started: false,
            ELinkResponses: 0,
            ZipResponses: 0
        });
        self.vmBtnToggleService = new vmBtnToggleService(this);
        self.kocStatus = ko.computed(function () {
            if (self.koServiceStatus().Contacted)
            {
                return ("Service is " + (self.koServiceStatus().Started ? "started" : "stopped") +  ", " + self.koServiceStatus().ELinkResponses + " Elink responses, " + self.koServiceStatus().ZipResponses + " Zip Responses");
            } else {
                return ("Contacting server...");
            }
        });
        GetStatus();
        setInterval(GetStatus, g_StatusPollRateSecs * 1000);

        function GetStatus() {
            var jqXhrResponse = $.get(g_StatusUri, {}, UpdateStatus).fail(function (jqXHR, sStatus) { UpdateStatus(null, sStatus, jqXHR); });
        }

        function UpdateStatus(_Response, _sStatus, _jqXHR) {
            if (_jqXHR.status === 200) {
                self.koServiceStatus({
                    Contacted: true,
                    Started: (_Response.Status === 1),
                    ELinkResponses: _Response.ElinkResponses,
                    ZipResponses: _Response.ZipResponses
                });
            } else {
                self.koServiceStatus({
                    Contacted: false,
                    Started: false,
                    ELinkResponses: 0,
                    ZipResponses: 0
                });
            }
        }
    }
    
    function vmBtnToggleService(_parent) {
        var self = this;
        self.kocClass = ko.computed(function () {
            return ("btn " + (_parent.koServiceStatus().Started ? "btn-danger" : "btn-success"));
        });
        self.kocIconClass = ko.computed(function () {
            return ("glyphicon " + (_parent.koServiceStatus().Started ? "glyphicon-stop" : "glyphicon-play"));
        });
        self.kocLabel = ko.computed(function () {
            return (_parent.koServiceStatus().Started ? "Stop" : "Start");
        });
        self.kocDisabled = ko.computed(function () {
            return (_parent.koServiceStatus().Contacted === false);
        });
        self.koeToggleService = function (i, e) {
            $.post((_parent.koServiceStatus().Started ? g_StopUri : g_StartUri), {}, ToggleServiceResponse);
        };
        
        function ToggleServiceResponse(_Response, _sStatus, _jqXHR) {
            if (_jqXHR.status === 200) {
                switch (_Response.toLowerCase())
                {
                    case "started":
                        {
                            _parent.koServiceStatus().Started = true;
                            break;
                        }
                    case "stopped":
                        {
                            _parent.koServiceStatus().Started = false;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
})();