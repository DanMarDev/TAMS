import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useNotification } from '../../context/NotificationContext';

export default function RegisterForm() {
  const { register } = useAuth();
  const { notify } = useNotification();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    email: '',
    userName: '',
    password: '',
    confirmPassword: '',
  });
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  const update = (key) => (e) => setForm({ ...form, [key]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (form.password !== form.confirmPassword) {
      setError('Passwords do not match');
      return;
    }
    if (form.password.length < 8) {
      setError('Password must be at least 8 characters');
      return;
    }

    setSubmitting(true);
    try {
      await register(form.email, form.userName, form.password, form.confirmPassword);
      notify('Account created', 'success');
      navigate('/dashboard', { replace: true });
    } catch (err) {
      const data = err.response?.data;
      const msg =
        data?.message ||
        (typeof data === 'string' ? data : null) ||
        'Registration failed';
      setError(msg);
      notify('Registration failed', 'error');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label className="block text-sm font-medium mb-1">Username</label>
        <input
          type="text"
          value={form.userName}
          onChange={update('userName')}
          required
          minLength={3}
          maxLength={255}
          autoComplete="username"
          className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
        />
      </div>

      <div>
        <label className="block text-sm font-medium mb-1">Email</label>
        <input
          type="email"
          value={form.email}
          onChange={update('email')}
          required
          autoComplete="email"
          className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
        />
      </div>

      <div>
        <label className="block text-sm font-medium mb-1">Password</label>
        <input
          type="password"
          value={form.password}
          onChange={update('password')}
          required
          minLength={8}
          autoComplete="new-password"
          className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
        />
      </div>

      <div>
        <label className="block text-sm font-medium mb-1">Confirm password</label>
        <input
          type="password"
          value={form.confirmPassword}
          onChange={update('confirmPassword')}
          required
          minLength={8}
          autoComplete="new-password"
          className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
        />
      </div>

      {error && <p className="text-sm text-red-600">{error}</p>}

      <button
        type="submit"
        disabled={submitting}
        className="w-full bg-slate-900 text-white py-2 rounded hover:bg-slate-800 disabled:opacity-50"
      >
        {submitting ? 'Creating account…' : 'Create account'}
      </button>
    </form>
  );
}
