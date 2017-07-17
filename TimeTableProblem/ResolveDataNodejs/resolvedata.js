var fs = require('fs');
var new4 = JSON.parse(fs.readFileSync('mData.json'));
var checknum = 0;
function toPlaceID(data) {

	if (data.grade < 3) {
		return data.grade - 1;
	}
	else {
		return data.grade + data.profession - 2;
	}
}


for (var index = 0; index < new4.length; index++) {
	var element = new4[index];
	element.PlaceID = toPlaceID(element);
}

var interestData = [];

function initInterestData(PlaceID) {
	interestData = [];
	for (var index = 0; index < new4.length; index++) {
		var element = new4[index];
		if (element.PlaceID == PlaceID) {
			interestData.push(element);
		}
	}
}

console.log("1");
var interestData2;
function init() {
	interestData2 = [];
	for (var index = 0; index < 5; index++) {
		for (var index2 = 0; index2 < 6; index2++) 	interestData2.push("NA");
	}
}

console.log("1");

function initMap(func) {
	init();
	for (var index = 0; index < interestData.length; index++) {
		var element = interestData[index];
		if (func(element)) {
			interestData2[(element.day - 1) * 6 + element.koma - 1] = element.name;
		}

	}
}
console.log("1");
function ShowMap() {
	for (var index = 0; index < 6; index++) {
		var tmp = "";
		for (var index2 = 0; index2 < 5; index2++) {
			tmp += interestData2[index2 * 6 + index] + "\t";
			if(interestData2[index2 * 6 + index]!="NA") checknum++;
	}
		
		console.log(tmp);
	}
}
console.log("1");

for (var index = 0; index < 5; index++) {
	initInterestData(index);
	console.log("Place"+index);
	initMap(function (c) { return c.semester == 1 });
	ShowMap();
	console.log();
	initMap(function (c) { return c.semester == 2 });
	ShowMap();
	console.log();
	initMap(function (c) { return c.semester == 3 });
	ShowMap();
	console.log();
}
console.log(checknum);

/*var fs = require('fs');
var new4 = JSON.parse(fs.readFileSync('new 4.json'));
var new3 = JSON.parse(fs.readFileSync('new 3.json'));
var out1 = [];
var out2 = [];
var out = [];
console.log(new4.length);
console.log(new3.length);
for (var i = 0; i < new4.length; i++) {
	if (contains(new4, new3[i])) out.push(new3[i]);
}
for (var i = 0; i < new4.length; i++) {
	if (contains(new3, new4[i])) out.push(new4[i]);
}

function contains(arr, datetime) {
	if (datetime == undefined) return;
	if (datetime.Datetime == undefined) return false;
	if (datetime.Mean_Temperature == "-") return false;
	for (var i = 0; i < new4.length; i++) {
		if (arr[i] == undefined) continue;
		if (arr[i].Datetime == undefined) continue;
		if (arr[i].Mean_Temperature == "-") continue;
		if (arr[i].Datetime == datetime.Datetime)
			return true;
	}

	return false;
}
fs.writeFile('out.json', JSON.stringify(out), {
	flag: "a+"
}, function (err, data) {
	if (err) {
		console.log("readFile file error");
		return false;

	}

	//console.log(data);
});*/
