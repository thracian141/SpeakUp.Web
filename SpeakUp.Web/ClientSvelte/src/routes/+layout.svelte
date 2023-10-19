<script lang='ts'>
    import Navbar from "./Navbar.svelte";
    import './styles.css';
</script>

<div class="app">
    <Navbar />

    <main>
        <slot />
    </main>
</div>

<style>
    .app {
		display: flex;
		flex-direction: row;
		min-height: 100vh;
	}

    main {
		flex: 1;
		display: flex;
		flex-direction: column;
		width: 100%;
		margin: 0;
        padding:5rem;
        padding-top: 2rem;
        padding-bottom:3rem;
		box-sizing: border-box;
	}
</style>