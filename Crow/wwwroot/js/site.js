// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Get the modal elements
var modal = document.getElementById('photoModal');
var modalImg = document.getElementById('modalImage');
var modalContentWrapper = document.getElementById('modalContentWrapper')
var closeModal = document.getElementsByClassName('close-modal')[0];

document.querySelectorAll('.photo-gallery-item').forEach(item => {

    item.addEventListener('click', function () {

        const imgElement = item.querySelector('.gallery-image');
        const imgSrc = imgElement ? imgElement.src : null;

        var id = item.getAttribute('data-id')
        var location = $(this).find('.photo-info-overlay p:nth-child(1)').text(); 
        var date = $(this).find('.photo-info-overlay p:nth-child(2)').text(); 
        var time = $(this).find('.photo-info-overlay p:nth-child(3)').text(); 
        var notes = $(this).find('.photo-info-overlay p:nth-child(4)').text(); 

        var detailsUrl = `/Photos/Edit/${id}`;
        var deleteUrl = `/Photos/Delete/${id}`

        $('#modalId').attr('href', detailsUrl);
        $('#modalImage').attr('src', imgSrc);
        $('#modalLocation').text(location);
        $('#modalDate').text(date);
        $('#modalTime').text(time);
        $('#modalNotes').text(notes); 

        if (!location || location.trim() === "") {
            $('#modalLocation').text('No location').addClass('text-muted');
        } else {
            $('#modalLocation').text(location).removeClass('text-muted');
        }

        if (!time || time.trim() === "") {
            $('#modalTime').text('No date').addClass('text-muted');
        } else {
            $('#modalTime').text(time).removeClass('text-muted');
        }

        if (!notes || notes.trim() === "") {
            $('#modalNotes').text('No notes').addClass('text-muted');
        } else {
            $('#modalNotes').text(notes).removeClass('text-muted');
        }


        if (imgSrc) {
            $('#photoModal').modal('show');
        } else {
            console.error('No image found inside the clicked item.');
        }
    });
});

closeModal.onclick = function () {
    $('#photoModal').modal('hide'); // Properly hide the modal using Bootstrap's modal method
};


const scrollableContent = document.querySelector('.scrollable-content');
const btnGroup = document.querySelector('.details-btn-group');

scrollableContent.addEventListener('scroll', function () {
    // Check if user has scrolled to the bottom
    const tolerance = 3;
    if (scrollableContent.scrollHeight - scrollableContent.scrollTop <= scrollableContent.clientHeight + tolerance) {
        // User is at the bottom, remove the shadow
        btnGroup.classList.add('no-shadow');
    } else {
        // User is not at the bottom, keep the shadow
        btnGroup.classList.remove('no-shadow');
    }
});