<script lang="ts">
    import { onMount } from 'svelte';
    import * as UserManager from '../../lib/scripts/UserManager';
    console.log('Script is running');

    let userPromise : Promise<UserManager.User> | null;
    let profilePicturePromise : Promise<string>;

    onMount(async () => {
        console.log('onMount is running');
        userPromise = UserManager.getUser();
        profilePicturePromise = userPromise
            .then(user => {
                if (user.profilePictureUrl) {
                    return UserManager.getPfp(user.profilePictureUrl);
                } else {
                    throw new Error("Profile picture URL is undefined");
                }
            }).then(pfp => {
                if (pfp) {
                    return pfp;
                } else {
                    throw new Error("Profile picture is undefined");
                }
            });
    });
</script>


{#await userPromise then user}
    <div class="userinfo-wrapper">
        {#await profilePicturePromise then pfp}
            <img src={pfp} alt="Avatar" class="avatar-xl" style="margin-top: 1rem;"/>
        {/await}
    </div>
{/await}

<style>
    .userinfo-wrapper {
        display: flex;
        flex-direction: column;
        height: 36rem;
        width: 70rem;
        border-radius: 12px;
        background-color: var(--el-bg-color);
        margin: auto;
        align-self: center;
        text-align: center;
        align-items: center;
        padding: 1rem;
        box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.2) !important;
    }
</style>