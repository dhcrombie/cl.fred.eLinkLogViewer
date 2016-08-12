(function () {
    $(document).ready(function () {
        ko.applyBindings(new vmLogViewer(), $(".LogViewer")[0]);
    });

    function vmLogViewer() {
        var self = this;
        var m_PageNo = 1;
        var m_Request = null;
        self.koPageSize = ko.observable(100);
        self.koLoadingItems = ko.observable(false);
        self.koFilterCover = ko.observable("");
        self.koFilterBranch = ko.observable("");
        self.koFilterClient = ko.observable("");
        self.koFilterClientNo = ko.observable("");
        self.koFilterFileCont = ko.observable("");
        self.koFilterDateFrom = ko.observable(new Date());
        self.koFilterDateTo = ko.observable(new Date());
        self.koaLogItems = ko.observableArray([]);
        self.koTotalItemCount = ko.observable(0);
        self.kocIsMoreItems = ko.computed(function () {
            return (self.koaLogItems().length < self.koTotalItemCount());
        });
        self.koeSearch = function (i, e) {
            Search();
        };
        self.koeLoadMore = function (i, e) {
            m_PageNo = m_PageNo + 1;
            GetLogEntries();
        };
        Initialise();
        
        function Initialise() {
            $(".DateFilter").datepicker({
                dateFormat: "dd/mm/yy",
                showAnim: "slideDown",
                changeMonth: true,
                changeYear: true,
                onSelect: function () {
                    $(this).change();
                }
            });
            $("#txtDateFrom, #txtDateTo").change(function () {
                var jqDateField = $(this);
                var Value = jqDateField.datepicker("getDate");
                switch (jqDateField.attr('id')) {
                    case "txtDateFrom":
                        {
                            self.koFilterDateFrom(Value);
                            break;
                        }
                    case "txtDateTo":
                        {
                            self.koFilterDateTo(Value);
                            break;
                        }
                }
            });
            $("#txtDateFrom").datepicker("setDate", self.koFilterDateFrom());
            $("#txtDateTo").datepicker("setDate", self.koFilterDateTo());
            Search();
        }
        
        function Search() {
            self.koaLogItems([]);
            m_PageNo = 1;
            GetLogEntries();
        }
        
        function GetLogEntries() {
            self.koLoadingItems(true);
            m_Request = $.post("/api/Log/",
                               {
                                   PageNo: m_PageNo,
                                   PageSize: self.koPageSize(),
                                   Filters: {
                                       DateFrom: self.koFilterDateFrom().toISOString(),
                                       DateTo: self.koFilterDateTo().toISOString(),
                                       Cover: self.koFilterCover(),
                                       Branch: self.koFilterBranch(),
                                       Client: self.koFilterClient(),
                                       ClientNumber: self.koFilterClientNo()
                                   }
                               },
                               UpdateLogEntries
                              ).fail(function (jqXHR, sStatus) { UpdateLogEntries(null, sStatus, jqXHR); });
        }
        
        function UpdateLogEntries(_Response, _sStatus, _jqXHR) {
            self.koLoadingItems(false);
            if (_jqXHR.status === 200) {
                self.koTotalItemCount(_Response.TotalResultCount);
                self.koaLogItems(self.koaLogItems().concat(_Response.Results));
            }
        }
    }
})();