import { createContext, useContext, useState, useCallback } from "react";

export const NotificationContext = createContext(null);

export function NotificationProvider({ children }) {
    const [toasts, setToasts] = useState([]);

    const remove = useCallback((id) => {
        setToasts((prev) => prev.filter((t) => t.id !== id));
    }, []);

    const notify = useCallback((message, type = "info", duration = 3000) => {
        const id = crypto.randomUUID();
        setToasts((prev) => [...prev, { id, message, type }]);
        setTimeout(() => remove(id), duration);
    }, [remove]);

    return (
        <NotificationContext.Provider value={{ notify }}>
            {children}
            <div className="fixed top-4 right-4 space-y-2 z-50">
                {toasts.map((toast) => (
                    <div 
                        key={toast.id} 
                        className={`px-4 py-2 rounded shadow text-white ${
                            toast.type === "error" ? "bg-red-500" 
                            : toast.type === "success" ? "bg-green-500" 
                            : "bg-slate-700"
                        }`}
                        onClick={() => remove(toast.id)}
                    >
                        {toast.message}
                    </div>
                ))}
            </div>
        </NotificationContext.Provider>
    );
}

export function useNotification() {
    const context = useContext(NotificationContext);
    if (!context) {
        throw new Error("useNotification must be used within a NotificationProvider");
    }
    return context;
}