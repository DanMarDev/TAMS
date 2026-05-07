import { useState } from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import TopBar from "./TopBar";

export default function Layout() {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    return (
        <div className="flex min-h-screen">
            <Sidebar open={isSidebarOpen} onClose={() => setIsSidebarOpen(false)} />
            <div className="flex-1 flex flex-col">
                <TopBar onMenuClick={() => setIsSidebarOpen(true)} />
                <main className="flex-1 p-6">
                    <Outlet />
                </main>
            </div>
        </div>
    );
}