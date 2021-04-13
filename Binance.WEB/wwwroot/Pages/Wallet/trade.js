var KTDatatablesDataSourceAjaxServer = function () {
    $.fn.dataTable.Api.register('column().title()', function () {
        return $(this.header()).text().trim();
    });
    var initmyWalletSummaryChart = function () {     
        var tblMyWalletTradeSummary = $('#tblMyWalletTradeSummary').DataTable({
            responsive: true,
            lengthMenu: [5, 10, 25, 50],
            pageLength: 10,
            order: [[5, "desc"]],
            buttons: [
                'excelHtml5'
            ],
            language: {
                url: datatablesLangUrl
            },
            searchDelay: 500,
            processing: true,
            serverSide: false,
            ajax: {
                url: getBaseUrl() + 'Binance/MyTradeList',

                contentType: 'application/json',
                dataSrc: ""
            },
           // data: walletData,
            fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.profit >0) {
                    $('td', nRow).css('background-color', '#c9f7f5');
                }
                else {
                    $('td', nRow).css('background-color', '#F8BFB3');
                }
            },
            columns: [          
                { data: 'asset' },
                { data: 'totalBuyQuantity' },
                {
                    data: 'totalBuyPrice', render: function (data) {
                        return data.toFixed(2);
                    }
                },
                { data: 'totalSellQuantity' },
                {
                    data: 'totalSellPrice', render: function (data) {
                        return data.toFixed(2);
                    }
                },
                { data: 'totalAccountQuantity' },
                {
                    data: 'totalAccountPrice', render: function (data) {
                        return data.toFixed(2);
                    }
                },
                {
                    data: 'profit', render: function (data) {
                        return data.toFixed(2);
                    }
                },
                { data: 'currency' },
            ]
        });
        $('#export_excel').on('click', function (e) {
            e.preventDefault();
            tblMyWalletTradeSummary.button(0).trigger();
        });
    };
    return {
        init: function () {
            initmyWalletSummaryChart();           
        },
    };

}();
$(document).ready(function () {

    KTDatatablesDataSourceAjaxServer.init();

});


