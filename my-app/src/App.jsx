import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import { NotificationProvider } from './context/NotificationContext';
import ProtectedRoute from './components/routing/ProtectedRoute';
import Layout from './components/layout/Layout';
import LoginPage from './pages/LoginPage';
import InventoryPage from './pages/InventoryPage';
import ItemDetailPage from './pages/ItemDetailPage';
import DashboardPage from './pages/DashboardPage';
import SettingsPage from './pages/SettingsPage';

function App() {
    return (
        <AuthProvider>
            <NotificationProvider>
                <BrowserRouter>
                    <Routes>
                        <Route path="/login" element={<LoginPage />} />
                        <Route
                            element={
                                <ProtectedRoute>
                                    <Layout />
                                </ProtectedRoute>
                            }
                        >
                            <Route path="/dashboard" element={<DashboardPage />} />
                            <Route path="/inventory" element={<InventoryPage />} />
                            <Route path="/inventory/:itemId" element={<ItemDetailPage />} />
                            <Route path="/settings" element={<SettingsPage />} />
                        </Route>
                        <Route path="*" element={<Navigate to="/dashboard" replace/>} />
                    </Routes>
                </BrowserRouter>
            </NotificationProvider>
        </AuthProvider>
    );
}

export default App;