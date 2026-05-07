import { useState } from 'react';
import { authApi } from '../../api/authApi';
import { useNotification } from '../../context/NotificationContext';

export default function ForgotPasswordModal({ open, onClose }) {
  const { notify } = useNotification();
  const [email, setEmail] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [sent, setSent] = useState(false);

  if (!open) return null;

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    try {
      await authApi.forgotPassword(email);
    } catch {
      // we don't want to leak whether the email is registered
    } finally {
      setSent(true);
      setSubmitting(false);
      notify('If that email exists, a reset link has been sent', 'success');
    }
  };

  const close = () => {
    setEmail('');
    setSent(false);
    onClose();
  };

  return (
    <div
      className="fixed inset-0 z-40 bg-black/40 flex items-center justify-center p-4"
      onClick={close}
    >
      <div
        className="bg-white rounded-lg shadow-lg w-full max-w-sm p-6"
        onClick={(e) => e.stopPropagation()}
      >
        <h2 className="text-lg font-semibold mb-4">Reset password</h2>

        {sent ? (
          <div className="space-y-4">
            <p className="text-sm text-slate-700">
              Check your inbox for password reset instructions.
            </p>
            <button
              onClick={close}
              className="w-full bg-slate-900 text-white py-2 rounded hover:bg-slate-800"
            >
              Close
            </button>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-4">
            <p className="text-sm text-slate-600">
              Enter your account email and we'll send you a reset link.
            </p>
            <input
              type="email"
              required
              autoFocus
              placeholder="you@example.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
            />
            <div className="flex gap-2">
              <button
                type="button"
                onClick={close}
                className="flex-1 border rounded py-2 hover:bg-slate-50"
              >
                Cancel
              </button>
              <button
                type="submit"
                disabled={submitting}
                className="flex-1 bg-slate-900 text-white py-2 rounded hover:bg-slate-800 disabled:opacity-50"
              >
                {submitting ? 'Sending…' : 'Send link'}
              </button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
}
