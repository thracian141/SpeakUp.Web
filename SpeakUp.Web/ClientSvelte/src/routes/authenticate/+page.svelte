<script lang="ts">
import {browser} from "$app/environment";
import {goto} from '$app/navigation';

let username = '';
let password = '';
let email = '';

async function handleSubmit(event: Event) {
    event.preventDefault();

    const model = {
        UserName: username,
        Password: password,
    };

    const response = await fetch('https://localhost:5000/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(model),
    });

    const data = await response.json();

    if (response.ok) {
        console.log("Login successful!");
        document.cookie = `token=${data.token};path=/;Secure;SameSite=Strict;`;
        await goto('/');
        location.reload();
    } else {
        console.error("Login failed: ", data.message);
    }
}
</script>

<div class="form-wrap" on:submit={handleSubmit}>
    <h2>Sign in</h2>
    <form method="post" style="position: relative;">
        <div class="form-section">
            <img src="src/lib/icons/usernameicon.svg" alt="Username Icon" class="form-icon"/>
            <p class:label-filled={username !== ''} class="form-label" >Username</p>
            <input type="text" class="form-control" bind:value={username}/>
        </div>
        <div class="form-section">
            <img src="src/lib/icons/emailicon.svg" alt="Username Icon" class="form-icon"/>
            <p class:label-filled={email !== ''} class="form-label" >Email</p>
            <input type="text" class="form-control" bind:value={email}/>
        </div>
        <div class="form-section">
            <img src="src/lib/icons/passwordicon.svg" alt="Username Icon" class="form-icon"/>
            <p class:label-filled={password !== ''} class="form-label" >Password</p>
            <input type="password" class="form-control" bind:value={password} />
        </div>
        <button type="submit" class="submit-button" style="position:absolute; bottom:10%; left:10%;">
            Log in
        </button>
        <a href="./account/" style="font-size: 11pt !important; position:absolute; bottom:15%; right:10%; gap:0.5rem; width:30%; overflow:hidden">Forgot your password?</a>
    </form>
</div>

<style>
form {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
    height: 100%;
}

.form-wrap {
    display: flex;
    flex-direction: column;
    height: 42vh;
    width: 35vw;
    border-radius: 12px;
    background-color: var(--el-bg-color);
    margin: auto;
    margin-bottom: 14rem;
    text-align: center;
    align-items: center;
    padding: 1rem;
    box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2) !important;
}

.form-section {
    background-color: var(--el-bg-color);
    margin-bottom: 0.2rem;
    border-bottom: solid;
    border-width: 1px;
    border-color: #32373f;
    transition: border-color 0.3s ease-in-out;
    display: flex;
    position: relative;
    width: 80%;
    height:16%;
}

.form-section:focus-within {
    border-color: var(--fg-color);
}

.form-icon {
    width: 1.6rem;
    height: auto;
    margin-left: 0.5rem;
}

.form-control {
    height: 100%;
    background-color: var(--el-bg-color);
    border: none;
    color: var(--fg-color);
    padding-right: 7rem;
    box-shadow: none;
    outline: none;
    transition: none;
    width: 100%;
}

.form-label {
    display: flex;
    color: #4e5366;
    position: absolute;
    right: 0%;
    top: 20%;
    font-size: 11pt;
    line-height: 0.5rem;
    transition: color 0.2s;
    margin-right: 1rem;
}

.label-filled {
    color: var(--fg-color-2) !important
}
</style>
