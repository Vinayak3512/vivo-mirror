import Cookies from "js-cookie";

const ACCESS_TOKEN_KEY = "access_token";
const REFRESH_TOKEN_KEY = "refresh_token";
const COOKIE_TOKEN_KEY = "auth_token";
const USER_KEY = "auth_user";

export type StoredUser = { id: string; email?: string; name?: string };

export function getStoredUser(): StoredUser | null {
	if (typeof window === "undefined") return null;
	try {
		const raw = window.localStorage.getItem(USER_KEY);
		return raw ? (JSON.parse(raw) as StoredUser) : null;
	} catch {
		return null;
	}
}

export function setStoredUser(user: StoredUser) {
	if (typeof window === "undefined") return;
	window.localStorage.setItem(USER_KEY, JSON.stringify(user));
}

export function clearStoredUser() {
	if (typeof window === "undefined") return;
	window.localStorage.removeItem(USER_KEY);
}

let inMemoryAccessToken: string | null = null;

export function getAccessToken(): string | null {
	if (typeof window === "undefined") return null;
	if (inMemoryAccessToken) return inMemoryAccessToken;
	const fromStorage = window.localStorage.getItem(ACCESS_TOKEN_KEY);
	if (fromStorage) {
		inMemoryAccessToken = fromStorage;
	}
	return inMemoryAccessToken;
}

export function setAccessToken(token: string) {
	if (typeof window === "undefined") return;
	inMemoryAccessToken = token;
	window.localStorage.setItem(ACCESS_TOKEN_KEY, token);
	// Set a non-HttpOnly cookie so middleware can read it (edge limitation)
	Cookies.set(COOKIE_TOKEN_KEY, token, { sameSite: "lax", secure: true });
}

export function clearAccessToken() {
	if (typeof window === "undefined") return;
	inMemoryAccessToken = null;
	window.localStorage.removeItem(ACCESS_TOKEN_KEY);
	Cookies.remove(COOKIE_TOKEN_KEY);
	clearStoredUser();
}

export function getRefreshToken(): string | null {
	if (typeof window === "undefined") return null;
	return window.localStorage.getItem(REFRESH_TOKEN_KEY);
}

export function setRefreshToken(token: string) {
	if (typeof window === "undefined") return;
	window.localStorage.setItem(REFRESH_TOKEN_KEY, token);
}

export function clearRefreshToken() {
	if (typeof window === "undefined") return;
	window.localStorage.removeItem(REFRESH_TOKEN_KEY);
}

export async function refreshAccessTokenSilently(): Promise<string | null> {
	if (typeof window === "undefined") return null;
	const baseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;
	const refreshToken = getRefreshToken();
	if (!baseUrl || !refreshToken) return null;
	try {
		const res = await fetch(`${baseUrl.replace(/\/$/, "")}/auth/refresh`, {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			body: JSON.stringify({ refreshToken }),
			credentials: "include",
		});
		if (!res.ok) return null;
		const data = (await res.json()) as { accessToken?: string; refreshToken?: string };
		if (data.accessToken) setAccessToken(data.accessToken);
		if (data.refreshToken) setRefreshToken(data.refreshToken);
		return data.accessToken ?? null;
	} catch {
		return null;
	}
}


