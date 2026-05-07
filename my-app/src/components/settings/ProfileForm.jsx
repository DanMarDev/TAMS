import { useEffect, useState } from 'react';
import { useNotification } from '../../context/NotificationContext';

export default function ProfileForm({ profile, onUpdate }) {
  const { notify } = useNotification();
  const [userName, setUserName] = useState('');
  const [email, setEmail] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (profile) {
      setUserName(profile.userName ?? '');
      setEmail(profile.email ?? '');
    }
  }, [profile]);

  const dirty =
    profile && (userName !== profile.userName || email !== profile.email);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    if (!userName.trim() || !email.trim()) {
      setError('Username and email are required');
      return;
    }
    setSubmitting(true);
    try {
      await onUpdate({ userName: userName.trim(), email: email.trim() });
      notify('Profile updated', 'success');
    } catch (err) {
      const msg = err.response?.data?.message || err.response?.data || 'Update failed';
      setError(msg);
      notify(msg, 'error');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <section className="bg-white rounded-lg shadow p-6 space-y-4">
      <h2 className="text-lg font-semibold">Profile</h2>
      <form onSubmit={handleSubmit} className="space-y-3">
        <div>
          <label className="block text-sm font-medium mb-1">Username</label>
          <input
            type="text"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            maxLength={255}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
          />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            maxLength={255}
            className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
          />
        </div>
        {error && <p className="text-sm text-red-600">{error}</p>}
        <div className="flex justify-end">
          <button
            type="submit"
            disabled={submitting || !dirty}
            className="bg-slate-900 text-white px-4 py-2 rounded hover:bg-slate-800 disabled:opacity-50"
          >
            {submitting ? 'Saving…' : 'Save Profile'}
          </button>
        </div>
      </form>
    </section>
  );
}
