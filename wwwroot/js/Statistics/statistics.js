$(document).ready(function () {

    $.ajax({
        type: 'GET',
        url: '/Statistics/TotalIncomeByBranch',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: (res) => loadIncomeByBranchGraph('#IncomeByBranchGraph', res),
    });

    $.ajax({
        type: 'GET',
        url: '/Statistics/TotalOrdersByProductType',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: (res) => loadOrdersByProductTypeGraph('#OrdersByProductTypeGraph', res),
    });

});

function loadIncomeByBranchGraph(element, data) {

    var colors = ['#1E29E1', '#DA790d', '#23702B'];
    var margin = ({ top: 50, right: 0, bottom: 50, left: 100 });
    var height = 350;
    var width = 700;
    var yAxis = g => g
        .attr("transform", `translate(${margin.left},0)`)
        .call(d3.axisLeft(y))
        .call(g => g.select(".domain").remove());
    var xAxis = g => g
        .attr("transform", `translate(0,${height - margin.bottom})`)
        .call(d3.axisBottom(x)
            .tickSizeOuter(0));
    var y = d3.scaleLinear()
        .domain([0, d3.max(data, d => d.income)]).nice()
        .range([height - margin.bottom, margin.top]);
    var x = d3.scaleBand()
        .domain(data.map(d => d.name))
        .range([margin.left, width - margin.right])
        .padding(0.1);

    var svg = d3.select(element)
        .append('svg')
        .attr('width', width)
        .attr('height', height);

    svg.append("g")
        .attr("fill", "steelblue")
        .selectAll("rect").data(data).enter().append("rect")
        .attr("x", d => x(d.name))
        .attr("y", d => y(d.income))
        .attr("height", d => y(0) - y(d.income))
        .attr("width", x.bandwidth())
        .attr("fill", (d, i) => { return colors[i % colors.length] });

    svg.append("g")
        .call(xAxis);

    svg.append("g")
        .call(yAxis);

    svg.selectAll("text.bar")
        .data(data)
        .enter().append("text")
        .attr("class", "bar")
        .attr("text", "middle")
        .attr("x", function (d) { return x(d.name); })
        .attr("y", function (d) { return y(d.income); })
        .text(function (d) { return (d.income); });

    svg.node();
}

function loadOrdersByProductTypeGraph(element, data) {

    var colors = ['#4DD0E1', '#DA790d', '#23702B', '#E1341E', '#E0B11F', '#B6DE21', '#23DC95', '#1ECBE1'];
    var margin = ({ top: 20, right: 0, bottom: 30, left: 100 });
    var height = 350;
    var width = 700;
    var yAxis = g => g
        .attr("transform", `translate(${margin.left},0)`)
        .call(d3.axisLeft(y))
        .call(g => g.select(".domain").remove());
    var xAxis = g => g
        .attr("transform", `translate(0,${height - margin.bottom})`)
        .call(d3.axisBottom(x)
            .tickSizeOuter(0));
    var y = d3.scaleLinear()
        .domain([0, d3.max(data, d => d.amount)])
        .range([height - margin.bottom, margin.top]);
    var x = d3.scaleBand()
        .domain(data.map(d => d.name))
        .range([margin.left, width - margin.right])
        .padding(0.1);

    var svg = d3.select(element)
        .append('svg')
        .attr('width', width)
        .attr('height', height);

    svg.append("g")
        .attr("fill", "steelblue")
        .selectAll("rect").data(data).enter().append("rect")
        .attr("x", d => x(d.name))
        .attr("y", d => y(d.amount))
        .attr("height", d => y(0) - y(d.amount))
        .attr("width", x.bandwidth())
        .attr("fill", (d, i) => { return colors[i % colors.length] });

    svg.append("g")
        .call(xAxis);

    svg.append("g")
        .call(yAxis);

    svg.selectAll("text.bar")
        .data(data)
        .enter().append("text")
        .attr("class", "bar")
        .attr("text", "middle")
        .attr("x", function (d) { return x(d.name); })
        .attr("y", function (d) { return y(d.amount); })
        .text(function (d) { return (d.amount); });

    svg.node();
}