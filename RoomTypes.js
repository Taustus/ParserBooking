var buttons = document.getElementsByClassName('u-display-inline-block hprt-facilities-facility');
for (var i = 0; i < buttons.length; i++) {
    buttons[i].click();
}
var tableNodes = document.getElementsByClassName('hprt-table')[0].childNodes[5].childNodes;
var stars = (document.getElementsByClassName('hp__hotel_ratings__stars nowrap')[0].childNodes[1] == undefined) ?
    0 : (document.getElementsByClassName('hp__hotel_ratings__stars nowrap')[0].childNodes[1].getAttribute('title') != null) ?
        document.getElementsByClassName('hp__hotel_ratings__stars nowrap')[0].childNodes[1].getAttribute('title').split('-')[0] :
        document.getElementsByClassName('hp__hotel_ratings__stars nowrap')[0].childNodes[1].getElementsByClassName('bk-icon -iconset-square_rating').length;
var id = document.querySelectorAll('[rel="alternate"]')[0].getAttribute('href').split('/hotel/')[1].split('?')[0];
stars = parseInt(stars);
var type = document.getElementsByClassName('hp__hotel-name')[0].childNodes[1].innerText;
var name = document.getElementsByClassName('hp__hotel-name')[0].childNodes[2].nodeValue.split('\n')[1];
var url = window.location.href;
var reviews = (document.getElementById('show_reviews_tab').childNodes[1].innerText.includes('нет')) ? 0 : document.getElementById('show_reviews_tab').childNodes[1].innerText.split('(')[1].split(')')[0];
if (reviews != 0) {
    reviews = parseInt(reviews.replace(' ', ''));
}
var rating = 0;
if (document.getElementsByClassName('bui-review-score__badge')[0] != undefined) {
    rating = document.getElementsByClassName('bui-review-score__badge')[0].innerText;
    rating = parseFloat(rating.replace(',', '.'));
}
var address = document.getElementsByClassName('address address_clean')[0].innerText.split(' –')[0];
var city = address.split(', ')[address.split(', ').length - 2];
address = address.split(', ' + city)[0];
var region = address.split(', ')[1];
var temp = address.split(', ');
if (temp.length > 2) {
    address = "";
    for (var i = 0; i < temp.length; i++) {
        if (i - 1 != temp.length && i > 0) {
            address += ', ';
        }
        address += temp[i];
    }
    region = temp[temp.length - 1];
}
var spa = 'No';
var hotel = {
    'Region': region,
    'City': city,
    'Name': name,
    'ID': id,
    'URL': url,
    'Address': address,
    'Rating': rating,
    'Reviews': reviews,
    'Type': type,
    'Stars': stars,
    'SPA': spa,
    'Rooms': []
};
var roomtype = '';
var counter = -1;
for (var i = 0; i < tableNodes.length; i++) {
    if (tableNodes[i].hasChildNodes()) {
        var temp = tableNodes[i].getElementsByClassName('hprt-roomtype-icon-link')[0];
        if (temp != undefined) {
            roomtype = temp.innerText;
            hotel.Rooms.push({
                'Price': [],
                'RoomType': roomtype,
                'Capacity': [],
                'Meal': [],
                'Refund': []
            });
            counter++;
        }
        var conditions = undefined;
        if (tableNodes[i].hasChildNodes()) {
            var meal = 'Нет';
            var refund = 'Нет';
            if (tableNodes[i].getElementsByClassName('hprt-roomtype-icon-link')[0] != undefined) {
                conditions = tableNodes[i].childNodes[7].childNodes[1].childNodes[1].childNodes;
            }
            else {
                conditions = tableNodes[i].childNodes[5].childNodes[1].childNodes[1].childNodes;
            }
            for (var k = 0; k < conditions.length; k++) {
                if (conditions[k].innerText != undefined) {
                    var text = conditions[k].innerText;
                    text = text.toLowerCase();
                    if (text.includes('завтрак') || text.includes('обед') || text.includes('ужин')) {
                        if (text.includes('включен')) {
                            meal = text;
                        }
                    }
                    if (text.includes('оплата не возвращается')) {
                        refund = 'Нет';
                    }
                    if (text.includes('отмена бронирования')) {
                        refund = 'Да';
                    }
                }
            }

            hotel.Rooms[counter].Price.push(parseInt(tableNodes[i].getElementsByClassName('bui-price-display__value prco-font16-helper')[0].innerText.split('руб.')[0].replace(' ', '')));
            var temp2 = tableNodes[i].getElementsByClassName('bui-u-sr-only')[0].innerText;
            if (temp2.includes(' - ')) {
                hotel.Rooms[counter].Capacity.push(parseInt(temp2.split(' - ')[1].split(' ')[0]));
            }
            else {
                hotel.Rooms[counter].Capacity.push(parseInt(temp2.split(' ')[2]));
            }
            hotel.Rooms[counter].Meal.push(meal);
            hotel.Rooms[counter].Refund.push(refund);
        }

    }
}
if (stars == undefined || isNaN(stars)) {
    stars = 0;
}
var possibleSpa = document.getElementsByClassName('hp_desc_important_facilities clearfix hp_desc_important_facilities--bui ')[0] != undefined ? document.getElementsByClassName('hp_desc_important_facilities clearfix hp_desc_important_facilities--bui ')[0].childNodes : 0;
for (var i = 0; i < possibleSpa.length; i++) {
    if (possibleSpa[i].innerText != undefined) {
        if (possibleSpa[i].innerText.includes('Спа')) {
            hotel.SPA = 'Yes';
        }
    }
}

JSON.stringify(hotel);