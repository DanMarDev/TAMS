import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useNotification } from '../../context/NotificationContext';

  export default function LoginForm({ onForgotPassword }) {
    const { login } = useAuth();
    const { notify } = useNotification();
    const navigate = useNavigate();
    const location = useLocation();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
      e.preventDefault();
      setError('');
      setSubmitting(true);
      try {
        await login(email, password);
        const from = location.state?.from?.pathname || '/dashboard';
        navigate(from, { replace: true });
      } catch (err) {
        const msg =
          err.response?.data?.message ||
          err.response?.data ||
          'Invalid email or password';
        setError(typeof msg === 'string' ? msg : 'Invalid email or password');
        notify('Login failed', 'error');
      } finally {
        setSubmitting(false);
      }
    };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label className="block text-sm font-medium mb-1">Email</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          autoComplete="email"
          className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
        />
      </div>

      <div>
        <label className="block text-sm font-medium mb-1">Password</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          minLength={8}
          autoComplete="current-password"
          className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
        />
      </div>

      {error && <p className="text-sm text-red-600">{error}</p>}

      <button
        type="submit"
        disabled={submitting}
        className="w-full bg-slate-900 text-white py-2 rounded hover:bg-slate-800 disabled:opacity-50"
      >
        {submitting ? 'Signing in…' : 'Log in'}
      </button>

      <button
        type="button"
        onClick={onForgotPassword}
        className="block mx-auto text-sm text-slate-600 hover:underline"
      >
        Forgot password?
      </button>
    </form>
  );
}