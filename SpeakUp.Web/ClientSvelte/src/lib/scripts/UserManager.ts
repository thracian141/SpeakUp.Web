import { browser } from "$app/environment";

export interface User {
    id: string;
    userName: string;
    email: string;
    displayName: string;
    profilePictureUrl: string;
    accountCreatedDate: string;
    lastDeck: string;
    [key: string]: string;
}

export async function getUser() {
    const cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    if (!cookie) {
        console.log('Token not found');
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch('https://localhost:5000/account/userinfo', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data = await response.json();
    return data.userInfo;
}

export async function isLoggedIn() {
    let cookie;
    if (browser){
        cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    }
    if (!cookie) {
        return false;
    }

    const token = cookie.split('=')[1];
    const response = await fetch('https://localhost:5000/account/userinfo', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });

    if (!response.ok) {
        if (browser){
            localStorage.setItem('isLoggedIn', 'false');
        }
        return false;
    }

    if (browser){
        localStorage.setItem('isLoggedIn', 'true');
    }
    return true;
}

export async function getPfp(profilePictureUrl: string) {
    let cookie;
    if (browser) {
        cookie = document.cookie.split('; ').find(row => row.startsWith('token'));
    }
    if (!cookie) {
        console.log('Token not found');
        return;
    }

    const token = cookie.split('=')[1];
    const response = await fetch(`https://localhost:5000/account/profilepicture?profilePictureUrl=${encodeURIComponent(profilePictureUrl)}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    
    if (!response.ok) {
        console.error('Failed to fetch profile picture');
        return;
    }

    const blob = await response.blob();
    const url = URL.createObjectURL(blob);
    return url;
}