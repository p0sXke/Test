
$(() => {

    LoadFlightData();
    LoadUserProfileData();
    LoadBookedData();

    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();
    connection.on("LoadFlights", function () {
        LoadFlightData();
    })
    connection.on("LoadUserProfiles", function () {
        LoadUserProfileData();
    })
    connection.on("LoadBookings", function () {
        LoadBookedData();
    })

    LoadFlightData();
    LoadUserProfileData();
    LoadBookedData();

    function LoadFlightData() {
        let tr = '';
        let les;

        $.ajax({
            url: '/Flights/GetFlights',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {

                    if (v.Canceled) {
                        les = 'canceled';
                    } else if (v.SeatNumber < 5) {
                        les = 'fewer';
                    } else {
                        les = '';
                    }

                        tr += `<tr class="${les}">
                            <td>${v.Departure}</td>
                            <td>${v.Destination}</td>
                            <td>${v.DateTime}</td>
                            <td>${v.StopNumber}</td>
                            <td>${v.SeatNumber}</td>
                            <td>${v.Canceled}</td>
                            <td>
                            <a href='../Flights/Details?id=${v.Id}'>Details</a>        
                            <a href='../BookedFlights/Create?id=${v.Id}'>Book</a>        
                            </td>

                    </tr>`
                })

                $("#flightTableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }

        });

    }

    function LoadUserProfileData() {
        var tr = '';

        $.ajax({
            url: '/UserProfiles/GetUsers',
            method: 'GET',
            success: (result) => {
                console.log(result);

                $.each(result, (k, v) => {

                    tr += `<tr>
                            <td>${v.FirstName}</td>
                            <td>${v.LastName}</td>
                            <td>${v.Email}</td>
                            <td>${v.UserName}</td>
                            <td>${v.PhoneNumber}</td>
                            <td>${v.Notification}</td>
                            <td>
                            <a href='../UserProfiles/Edit?id=${v.Id}'>Edit</a>        
                            <a href='../UserProfiles/Details?id=${v.Id}'>Details</a>        
                            <a href='../UserProfiles/Delete?id=${v.Id}'>Delete</a>        
                            </td>
                    </tr>`
                })

                $("#userTableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }

        });

    }

    function LoadBookedData() {
        var tr = '';

        $.ajax({
            url: '/BookedFlights/GetBookings',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {

                    tr += `<tr>
                            <<td>${v.FlightDeparture}</td>
                            <td>${v.FlightDestination}</td>
                            <td>${v.FlightDateTime}</td>
                            <td>${v.FlightStopNumber}</td>
                            <td>${v.FlightSeatNumber}</td>
                            <td>${v.UserProfileFullName}</td>
                            <td>${v.UserProfileEmail}</td>
                            <td>${v.BookSeats}</td>
                            <td>${v.BookingStatus}</td>
                            <td>
                            <a href='../BookedFlights/ApproveReject?id=${v.Id}'>Approve/Reject</a>        
                            </td>

                    </tr>`
                })

                $("#bookedTableBody").html(tr);
            },
            error: (error) => {
                console.log(error)
            }

        });

    }
})