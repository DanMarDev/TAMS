import { useState, useEffect } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import LoginForm from '../components/auth/LoginForm';
import RegisterForm from '../components/auth/RegisterForm';
import ForgotPasswordModal from '../components/auth/ForgotPasswordModal';

export default function LoginPage() {
  const { isAuthenticated } = useAuth();
  const [mode, setMode] = useState('login');
  const [forgotOpen, setForgotOpen] = useState(false);

  useEffect(() => {
    document.title = mode === 'login' ? 'Log in · TAMS' : 'Sign up · TAMS';
  }, [mode]);

  if (isAuthenticated) return <Navigate to="/dashboard" replace />;

  return (
    <div className="min-h-screen flex items-center justify-center bg-slate-50 p-4">
      <div className="w-full max-w-sm bg-white rounded-lg shadow p-6">
        <h1 className="text-2xl font-semibold mb-6 text-center">
          {mode === 'login' ? 'Log in to TAMS' : 'Create your TAMS account'}
        </h1>

        {mode === 'login' ? (
          <LoginForm onForgotPassword={() => setForgotOpen(true)} />
        ) : (
          <RegisterForm />
        )}

        <div className="mt-6 text-center text-sm text-slate-600">
          {mode === 'login' ? (
            <>
              Don't have an account?{' '}
              <button
                onClick={() => setMode('register')}
                className="text-slate-900 font-medium hover:underline"
              >
                Sign up
              </button>
            </>
          ) : (
            <>
              Already have an account?{' '}
              <button
                onClick={() => setMode('login')}
                className="text-slate-900 font-medium hover:underline"
              >
                Log in
              </button>
            </>
          )}
        </div>
      </div>

      <ForgotPasswordModal open={forgotOpen} onClose={() => setForgotOpen(false)} />
    </div>
  );
}
