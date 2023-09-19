var croppie;

function initializeCroppie() {
    var input = document.getElementById('input-image');
    var profileImage = document.getElementById('profile-image');
    var croppedImagePreview = document.getElementById('cropped-image-preview');

    input.addEventListener('change', function () {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                if (croppie) {
                    croppie.destroy();
                }

                profileImage.style.display = 'none'; // Hide the original profile image

                croppie = new Croppie(croppedImagePreview, {
                    viewport: { width: 200, height: 200 },
                    boundary: { width: 200, height: 200 },
                    showZoomer: true
                });

                croppie.bind({
                    url: e.target.result
                });

                croppedImagePreview.style.display = 'block'; // Show the Croppie preview container
                document.getElementById('crop-section').style.display = 'block';
            };
            reader.readAsDataURL(input.files[0]);
        }
    });

    document.getElementById('save-image-button').addEventListener('click', function () {
        if (croppie) {
            croppie.result('blob').then(function (blob) {
                var croppedFile = new File([blob], 'cropped.jpg', { type: 'image/jpeg' });

                var formData = new FormData(document.getElementById('profile-form'));
                formData.set('Input.CroppedProfilePicture', croppedFile);

                fetch(document.getElementById('profile-form').action, {
                    method: 'POST',
                    body: formData
                }).then(function (response) {
                    if (response.ok) {
                        croppie.destroy();
                        document.getElementById('crop-section').style.display = 'none';
                        profileImage.src = URL.createObjectURL(croppedFile); // Update the profile image source
                        profileImage.style.display = 'block'; // Show the updated profile image
                        croppedImagePreview.style.display = 'none'; // Hide the Croppie preview container
                    } else {
                        // Handle the response if needed
                    }
                }).catch(function (error) {
                    // Handle errors if needed
                });
            });
        }
    });
}

document.addEventListener('DOMContentLoaded', initializeCroppie);