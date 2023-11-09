<script lang='ts'>
    import { browser } from '$app/environment';
	import { page } from '$app/stores';
    import {onMount} from "svelte";
    import * as UserManager from "../lib/scripts/UserManager";
    import './styles.css';
    
    let isExpanded = false;
    let isLogoutConfirmed = false;
    let isLoggedIn = UserManager.isLoggedIn();

    if (browser && localStorage.getItem('isExpanded') === "true") {
        isExpanded = true;
    }
    function logout() {
        console.log("Test function called");
        if (browser){
            document.cookie = 'token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
            location.href = '/';
            location.reload();
        }
    }
    function handleLogoutClick(event: Event) {
        event.preventDefault();
        if (isLogoutConfirmed) {
            logout();
            isLogoutConfirmed = false;

            if (browser){
                localStorage.removeItem('user');
                localStorage.removeItem('isLoggedIn');
            }
        } else {
            isLogoutConfirmed = true;
        }
    }
    function enableExpansion() {
        if (browser) {
            isExpanded = true;
            localStorage.setItem('isExpanded', String(isExpanded));
        }
    }
    function disableExpansion() {
        if (browser) {
            isExpanded = false;
            localStorage.setItem('isExpanded', String(isExpanded));
        }
    } 
    let currentPath;
    onMount(async () => {
        currentPath = window.location.pathname;
    });
</script>


    <nav id="nav" class={isExpanded ? 'nav-expanded' : ''} on:mouseenter={enableExpansion} on:mouseleave={disableExpansion}>
        <div class="logo" style="display: flex; flex-direction:row;">
            <img src="src/lib/icons/speakuplogo.svg" alt="SpeakUp Logo"/>
            <p>SPEAKUP</p>
        </div>
        <ul id="mainOptions" style="list-style-type:none; padding: 12px;">
			<a href="/" class="nav-option" class:active={$page.url.pathname == "/"}>
                <img src="src/lib/icons/homeicon.svg" alt="Home Icon" /><p>Home</p>
            </a>
			<a href="/learn" class="nav-option" class:active={$page.url.pathname == "/learn"}>
                <img src="src/lib/icons/cardsicon.svg" alt="Learn Icon" /><p>Learn</p>
            </a>
            <a href="/decks" class="nav-option" class:active={$page.url.pathname == "/decks"}>
                <img src="src/lib/icons/decksicon.svg" alt="Decks Icon" /><p>Decks</p>
            </a>
            <a href="/account" class="nav-option" class:active={$page.url.pathname == "/account"}>
                <img src="src/lib/icons/accounticon.svg" alt="Account Icon" /><p>Account</p>
            </a>
        </ul>
        {#await isLoggedIn then bool}
            {#if !bool}
                <ul style="list-style-type:none; padding: 12px; margin-top:auto">
                    <a id="authForm" class="nav-option" href="/authenticate" class:active={$page.url.pathname == "/authenticate"}>
                        <img src="src/lib/icons/loginicon.svg" alt="Authenticate Icon" />
                        <p style="text-overflow:clip; white-space:nowrap">Sign In</p>
                    </a>
                </ul>
            {:else}
                <ul style="list-style-type:none; padding: 12px; margin-top:auto">
                    <a id="authForm" class="nav-option" href="/" class:active={isLogoutConfirmed} on:click|preventDefault={handleLogoutClick}>
                        <img src="src/lib/icons/logouticon.svg" alt="Authenticate Icon" />
                        <p style="text-overflow:clip; white-space:nowrap">{isLogoutConfirmed ? 'Confirm' : 'Sign Out'}</p>
                    </a>
                </ul>
            {/if}
        {/await}

    </nav>


<style>
    .logo {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        padding-left: 1.2rem; padding-top: 1.2rem; padding-right: 1.2rem;
    }
    .logo p {
        font-weight: bold;
        font-size: 1rem;
        opacity: 0;
        transition: opacity 0.2s ease-in-out;
    }
    .logo img {
        margin-right:0.2rem;
        width:auto;
        height:3.3rem;
    }
    nav {
        width: 5.5rem;
        height: 100vh;
        background-color: var(--el-bg-color);
        transition: width 0.3s ease-in-out;
        font-weight: bold;
        display: flex;
        flex-direction: column;
        overflow: hidden;
    }
    .nav-expanded {
        width: 12rem;
    }

    .nav-option {
        display:flex;
        flex-direction: row;
        padding-left: 0.9rem; padding-right: 1rem;
        font-size:1rem;
        justify-content: space-between;
        border-radius: 12px;
        width:2.2rem;
        height: 3.8rem;
        transition: background-color 0.2s ease-in-out, width 0.2s ease-in-out;
        align-items: center;
    }
    .active {
        background-color: var(--selected-color) !important;
    }
    .active p, .active img {
        filter: brightness(0) saturate(100%) invert(100%) sepia(13%) saturate(6304%) hue-rotate(178deg) brightness(82%) contrast(93%);
    }
    .nav-option > img {
        height:60%;
    }
    .nav-option p {
        font-size: 0.9rem;
        opacity: 0;
        transition: opacity 0.2s ease-in-out;
        line-height: 2rem;
    }
    nav:hover .nav-option p, nav:hover .logo p {
        opacity: 1;
    }
    nav:hover .nav-option {
        width:8.5rem;
    }
    .nav-option:hover {
        background-color: var(--bg-color);
    }
</style>