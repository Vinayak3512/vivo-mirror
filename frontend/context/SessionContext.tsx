'use client';

import { createContext, PropsWithChildren, useCallback, useContext, useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { loginWithPassword, logout } from "../lib/auth/authService";
import { setCredentials, clearCredentials, AuthUser } from "../store/authSlice";
import { getAccessToken, getStoredUser, setStoredUser } from "../lib/auth/tokenStorage";

export type SessionContextValue = {
	isAuthenticated: boolean;
	isLoading: boolean;
	user: AuthUser | null;
	login: (args: { email: string; password: string }) => Promise<void>;
	logout: () => Promise<void>;
};

const SessionContext = createContext<SessionContextValue | undefined>(undefined);

export function SessionProvider({ children }: PropsWithChildren) {
	const dispatch = useDispatch();
	const [user, setUser] = useState<AuthUser | null>(null);
	const [isLoading, setIsLoading] = useState(true);
	const isAuthenticated = Boolean(user);

	// Rehydrate session from persisted storage on initial mount so a page
	// refresh doesn't lose the logged-in state even though a valid token exists.
	useEffect(() => {
		const token = getAccessToken();
		const storedUser = getStoredUser();
		if (token && storedUser) {
			setUser(storedUser);
			dispatch(setCredentials({ user: storedUser }));
		}
		setIsLoading(false);
	}, [dispatch]);

	const login = useCallback(async ({ email, password }: { email: string; password: string }) => {
		const res = await loginWithPassword(email, password);
		const nextUser: AuthUser | null =
			res.user ? { id: res.user.id, name: res.user.name, email: res.user.email } : null;
		setUser(nextUser);
		if (nextUser) setStoredUser(nextUser);
		dispatch(setCredentials({ user: nextUser }));
	}, [dispatch]);

	const signOut = useCallback(async () => {
		await logout();
		setUser(null);
		dispatch(clearCredentials());
	}, [dispatch]);

	const value = useMemo<SessionContextValue>(() => {
		return {
			isAuthenticated,
			isLoading,
			user,
			login,
			logout: signOut,
		};
	}, [isAuthenticated, isLoading, login, signOut, user]);

	return <SessionContext.Provider value={value}>{children}</SessionContext.Provider>;
}

export function useSession() {
	const ctx = useContext(SessionContext);
	if (!ctx) throw new Error("useSession must be used within SessionProvider");
	return ctx;
}


