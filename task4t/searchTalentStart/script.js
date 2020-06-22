$('#search').keyup(function () {
    //get data from json file
    var urlForJson = "data.json";


    //get data from Restful web Service in development environment
    //var urlForJson = "http://localhost:9000/api/talents";

    //get data from Restful web Service in production environment
    //var urlForJson= "http://csc123.azurewebsites.net/api/talents";

    //Url for the Cloud image hosting
    var urlForCloudImage = "http://res.cloudinary.com/dna8wj8ws/image/upload/v1590946527/"; 
    var searchField = $('#search').val();
    var myExp = new RegExp(searchField, "i");
    var isempty = true;
    $.getJSON(urlForJson, function (data) {
        var output = '<ul class="searchresults">';
        isempty = true;
        $.each(data, function (key, val) {
            //for debug
           
            if ((val.Name.search(myExp) != -1) ||
                (val.Bio.search(myExp) != -1)) {
                isempty = false;
                output += '<li>';
                output += '<h2>' + val.Name + '</h2>';
                //get the absolute path for local image
                //output += '<img src="images/'+ val.ShortName +'_tn.jpg" alt="'+ val.Name +'" />';

                //get the image from cloud hosting
                console.log(urlForCloudImage + val.ShortName + "_tn.jpg ");
                output += '<img src=' + urlForCloudImage + val.ShortName + "_tn.jpg alt=" + val.Name + '" />';
                output += '<p>' + val.Bio + '</p>';
                output += '</li>';
            }  
        });

        if (isempty == true) {
            output += '<li>No talent under this search</li>';

        }
        output += '</ul>';
        
        $('#update').html(output);
    }); //get JSON
});
