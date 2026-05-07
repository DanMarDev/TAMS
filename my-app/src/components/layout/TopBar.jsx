import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';

export default function TopBar({ onMenuClick }) {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = async () => {
        await logout();
        navigate('/login');
    };

    return (
        <header className="h-14 border-b bg-white flex items-center justify-between px-4">
        <button className="md:hidden p-2" onClick={onMenuClick} aria-label="Open menu">
          ☰
        </button>
        <div className="flex-1" />
        <div className="flex items-center gap-3">
          <span className="text-sm text-slate-600">{user?.userName}</span>
          <button
            onClick={handleLogout}
            className="text-sm px-3 py-1 rounded border hover:bg-slate-50"
          >
            Log out
          </button>
        </div>
      </header>
    );
}