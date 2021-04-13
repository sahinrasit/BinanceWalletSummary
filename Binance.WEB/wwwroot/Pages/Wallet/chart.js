var walletData = [];
var table;
var KTDatatablesDataSourceAjaxServer = function () {
    $.fn.dataTable.Api.register('column().title()', function () {
        return $(this.header()).text().trim();
    });
    var initQrReadDaily = function () {
        var table = $('#myWalletSummaryChart').DataTable({
            responsive: true,
            lengthMenu: [5, 10, 25, 50],
            pageLength: 10,
            order: [[5, "desc"]],
            language: {
                url: datatablesLangUrl
            },
            buttons: [
                'excelHtml5'
            ],
            searchDelay: 500,
            processing: true,
            serverSide: false,
            //ajax: {
            //    url: getBaseUrl() + 'Binance/MyAccountInfo',
                
            //    contentType: 'application/json',
            //    dataSrc: ""
            //},
            data: walletData,
            fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                if (aData.asset == "TOTAL" ) {
                    $('td', nRow).css('background-color', '#20c997');
                }
            },
            columns: [
                { data: 'asset' },
               
                {
                    data: 'free', render: function (data) {
                        return data.toFixed(4);
                    }
                },
                { data: 'locked' },
                {
                    data: 'unitValueUsdt', render: function (data) {
                        return data.toFixed(2);
                    }
                },
                {
                    data: 'usdtTotal', render: function (data) {
                        return data.toFixed(2);
                    }
                },
                {
                    data: 'portfolioPercentage', render: function (data) {
                        return "%"+data.toFixed(2);
                    }
                }

            ]
        });
        $('#export_excel').on('click', function (e) {
            e.preventDefault();
            table.button(0).trigger();
        });
        //$('#myWalletSummaryChart thead tr').clone(true).appendTo('#myWalletSummaryChart thead');
        //$('#myWalletSummaryChart thead tr:eq(1) th').each(function (i) {
        //    var title = $(this).text();
        //    $(this).html('<input type="text" placeholder="Search ' + title + '" />');

        //    $('input', this).on('keyup change', function () {
        //        if (table.column(i).search() !== this.value) {
        //            table
        //                .column(i)
        //                .search(this.value)
        //                .draw();
        //        }
        //    });
        //});
    };
    return {
        //main function to initiate the module
        init: function () {
            initQrReadDaily();           
        },
    };

}();
var AMchartFunc = function () { 
    var initQrReadLocationPie = function () {
        am4core.ready(function () {
            am4core.useTheme(am4themes_animated);
            var chart = am4core.create("chartPieMyWallet", am4charts.PieChart);
            //chart.dataSource.requestOptions.requestHeaders = [{
            //    "key": "Authorization",
            //    "value": getToken()
            //}];
            // Add data
            chart.data = walletData.filter(function (item) { return (item.asset != "TOTAL"); });  // Add data      

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "usdtTotal";
            pieSeries.dataFields.category = "asset";
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeOpacity = 1;

            // This creates initial animation
            pieSeries.hiddenState.properties.opacity = 1;
            pieSeries.hiddenState.properties.endAngle = -90;
            pieSeries.hiddenState.properties.startAngle = -90;

            chart.hiddenState.properties.radius = am4core.percent(0);

            // Chart title
            var title = chart.titles.create();
            title.text = "Benim Cüzdanım";
            title.fontSize = 20;
            title.paddingBottom = 10;
        }); // end am4core.ready()
    };
    return {
        //main function to initiate the module
        init: function () {          
            initQrReadLocationPie();
        },

    };
}();

$(document).ready(function () {
    //setInterval(function () {
    //    GeneralRequest.init();
    //}, 60000);
    $.ajax({
        type: "GET",
        url: getBaseUrl() + 'Binance/MyAccountInfo',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            walletData = data;
            AMchartFunc.init();
            KTDatatablesDataSourceAjaxServer.init();
            var totalMoney = walletData.filter(function (item) { return (item.asset == "TOTAL") })[0];
            $("#totalMoney").html(totalMoney.usdtTotal.toFixed(2) + "$");
            $("#totalMoneyTry").html((totalMoney.usdtTotal * 8.17).toFixed(2) + "₺");
        }
    });

});


