<script lang="ts">
    let message = 'Loading...'; // Initialize with a loading message.
    
    async function fetchMessage() {
        try {
            const response = await fetch('https://localhost:5000/api/test', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                message = await response.text(); // Update the message with the response from the API.
            } else {
                message = `Error: ${response.statusText}`;
            }
        } catch (error) { // No explicit type annotation
            const err = error as Error; // Use type assertion
            message = `Error: ${err.message}`;
        }
    }

    // Call the fetchMessage function when the component loads.
    fetchMessage();
</script>

<h1>{message}</h1>