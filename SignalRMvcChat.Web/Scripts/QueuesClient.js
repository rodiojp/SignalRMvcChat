/// <reference path='jquery-3.6.0.intellisense.js' />
/// <reference path='highcharts/highcharts.src.js' />
/// <reference path='moment/moment.js'/>
$(function () {
    window.Highcharts.setOptions({
        global: {
            useUTC: false
        }
    });

    var queueGraphes = [['rdm_ReadingRawDy', 'graph1'], ['rdm_ReadingRawHy', 'graph2']];
    var graphs = [];
    $(document).ready(function () {
        $.each(queueGraphes, function (ii, val) {
            graphs[ii] = new Highcharts.Chart({
                chart: {
                    renderTo: val[1],
                    type: 'line',
                    margin: [50, 10, 10, 50]
                },
                title: {
                    text: val[0]
                },
                xAxis: {
                    type: 'datetime',
                    tickInterval: 500,
                    labels: {
                        enabled: false
                    }
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                legend: {
                    enabled: false
                },
                exporting: {
                    enabled: false
                },
                series: [{
                    name: 'Processing Queues',
                    data: [],
                    animation: false
                }]
            });
        });
        $.connection.hub.logging = true;
        //Reference the auto-generated proxy for the hub
        var queueTicker = $.connection.processingQueuesHub;
        //Create a function that the hub can call back to display messages
        queueTicker.client.updateQueuesCounts = function (data) {
            console.log(data);
            $.each(data, function (ii, item) {
                var series;
                switch (item.QueueName) {
                    case 'rdm_ReadingRawDy':
                        series = graphs[0].series[0];
                        break;
                    case 'rdm_ReadingRawHy':
                        series = graphs[1].series[0];
                        break;
                    default: try {
                        console.log('No chart for: [' + item.QueueName + ']');
                    } catch (e) {

                    }
                }
                var x = Date.parse(moment(item.Date).toDate()),
                    y = item.Length;
                if (series != undefined) {
                    series.addPoint([x, y], true, false);
                }
            });
        };
        //Start connection
        $.connection.hub.start(function () {
            queueTicker.server.start()
                .done(function (success) {
                    try {
                        if (success == false) {
                            console.log('Failed to connect to server');
                        }
                        console.log('connected with ID = ' + $.connection.hub.id);
                    } catch (e) {

                    }
                });
        })
            .done(function (data) {
                console.log({ 'Connected ': data });
            });
    });
});
