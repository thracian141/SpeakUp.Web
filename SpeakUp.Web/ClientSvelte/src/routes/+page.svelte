<script lang='ts'>
    import { onMount } from 'svelte';
    import { browser } from '$app/environment';
    import * as UserManager from '../lib/scripts/UserManager';

    let message = '';
    onMount(async () => {
        const response = await fetch('https://localhost:5000/index/home');

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        message = data.message;
    });

    let userPromise = UserManager.getUser();
</script>

{#await userPromise then user}
<h1>Welcome, {user?.userName}!</h1>
<p>{message}</p>
{/await}