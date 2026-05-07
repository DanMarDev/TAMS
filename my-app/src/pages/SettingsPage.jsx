import { useUserSettings } from '../hooks/useUserSettings';
import ProfileForm from '../components/settings/ProfileForm';
import SellThresholdSlider from '../components/settings/SellThresholdSlider';
import CategoryManager from '../components/settings/CategoryManager';
import BrandManager from '../components/settings/BrandManager';
import PasswordChange from '../components/settings/PasswordChange';

export default function SettingsPage() {
  const { profile, loading, error, updateProfile, updateSellThreshold, changePassword } =
    useUserSettings();

  if (loading) return <div className="p-2">Loading…</div>;
  if (error) return <div className="p-2 text-red-600">{String(error)}</div>;

  return (
    <div className="space-y-6 max-w-3xl">
      <h1 className="text-2xl font-semibold">Settings</h1>
      <ProfileForm profile={profile} onUpdate={updateProfile} />
      <SellThresholdSlider profile={profile} onUpdate={updateSellThreshold} />
      <CategoryManager />
      <BrandManager />
      <PasswordChange onChangePassword={changePassword} />
    </div>
  );
}
