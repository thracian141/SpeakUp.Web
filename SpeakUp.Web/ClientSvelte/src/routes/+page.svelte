<script>
    import { writable } from 'svelte/store';

    export const user = writable({
        username: '', // Initialize with an empty string
    });
</script>

<h1>Welcome to SvelteKit</h1>
<p>Visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to read the documentation</p>
