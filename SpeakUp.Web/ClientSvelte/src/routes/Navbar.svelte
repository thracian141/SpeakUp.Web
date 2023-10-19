<script lang='ts'>
    import { browser } from '$app/environment';
	import { page } from '$app/stores';
    import {onMount} from "svelte";
    import './styles.css';
    
    let isExpanded = false;
    
    if (browser && localStorage.getItem('isExpanded') === "true") {
        isExpanded = true;
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
    onMount(() => {
        currentPath = window.location.pathname;
    });
</script>

<header>
    <nav id="nav" class={isExpanded ? 'nav-expanded' : ''} on:mouseenter={enableExpansion} on:mouseleave={disableExpansion}>
        <div class="logo">
            <img src="src/lib/icons/speakuplogo.svg" alt="SpeakUp Logo"/>
            <p>SpeakUP</p>
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
        <ul style="list-style-type:none; padding: 12px; margin-top:auto">
            <a id="authForm" class="nav-option" href="/authenticate" class:active={$page.url.pathname == "/authenticate"}>
                <img src="src/lib/icons/loginicon.svg" alt="Authenticate Icon" />
                <p style="text-overflow:clip; white-space:nowrap">Sign In</p>
            </a>
        </ul>
    </nav>
</header>

<style>
    .logo {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        padding-left: 1.2rem; padding-top: 1.2rem; padding-right: 1.2rem;
    }
    .logo p {
        font-weight: bold;
        font-size: 16pt;
        opacity: 0;
        transition: opacity 0.2s ease-in-out;
    }
    .logo img {
        width:3.6rem;
        height:auto;
    }
    header {
        display:flex;
    }
    nav {
        width: 5.5vw;
        height: 100%;
        background-color: var(--el-bg-color);
        transition: width 0.3s ease-in-out;
        font-weight: bold;
        display: flex;
        flex-direction: column;
        overflow: hidden;
    }
    .nav-expanded {
        width: 11vw;
    }

    .nav-option {
        display:flex;
        flex-direction: row;
        padding-left: 1.2rem; padding-right: 1.2rem;
        font-size:14pt;
        justify-content: space-between;
        border-radius: 12px;
        height:4rem;
        transition: background-color 0.2s ease-in-out;
    }
    .active {
        background-color: var(--selected-color) !important;
    }
    .active p, .active img {
        filter: brightness(0) saturate(100%) invert(100%) sepia(13%) saturate(6304%) hue-rotate(178deg) brightness(82%) contrast(93%);
    }
    .nav-option > img {
        width:2.4rem;
        height:auto;
    }
    .nav-option p {
        opacity: 0;
        transition: opacity 0.2s ease-in-out;
    }
    nav:hover .nav-option p, nav:hover .logo p {
        opacity: 1;
    }
    .nav-option:hover {
        background-color: var(--bg-color);
    }
</style>