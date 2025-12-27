import { useState } from 'react';
import Modal from './Modal';

const ApprovalModal = ({ isOpen, onClose, onSubmit, expense, isApproval = true }) => {
  const [notes, setNotes] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Red için not zorunlu
    if (!isApproval && !notes.trim()) {
      alert('Red sebebi zorunludur!');
      return;
    }

    setLoading(true);
    try {
      await onSubmit(notes.trim() || null);
      setNotes('');
      onClose();
    } catch (error) {
      console.error('Approval error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setNotes('');
    onClose();
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={handleClose}
      title={isApproval ? 'Gideri Onayla' : 'Gideri Reddet'}
    >
      <form onSubmit={handleSubmit}>
        {/* Gider Bilgileri */}
        <div className="card mb-3">
          <div className="card-body">
            <h6 className="card-title mb-3">Gider Detayları</h6>
            <div className="row">
              <div className="col-md-6 mb-2">
                <small className="text-muted">Kategori</small>
                <div><strong>{expense?.category}</strong></div>
              </div>
              <div className="col-md-6 mb-2">
                <small className="text-muted">Tutar</small>
                <div>
                  <strong>
                    {new Intl.NumberFormat('tr-TR', {
                      style: 'currency',
                      currency: expense?.currency === 'TL' ? 'TRY' : (expense?.currency || 'TRY')
                    }).format(expense?.amount || 0)}
                  </strong>
                  {expense?.currency !== 'TL' && (
                    <small className="text-muted ms-2">
                      (≈ {new Intl.NumberFormat('tr-TR', {
                        style: 'currency',
                        currency: 'TRY'
                      }).format(expense?.amountInTL || 0)})
                    </small>
                  )}
                </div>
              </div>
              <div className="col-md-6 mb-2">
                <small className="text-muted">Mağaza</small>
                <div>{expense?.storeName || '-'}</div>
              </div>
              <div className="col-md-6 mb-2">
                <small className="text-muted">Çalışan</small>
                <div>{expense?.employeeName || '-'}</div>
              </div>
              <div className="col-12 mb-2">
                <small className="text-muted">Açıklama</small>
                <div>{expense?.description}</div>
              </div>
              {expense?.vendor && (
                <div className="col-12 mb-2">
                  <small className="text-muted">Tedarikçi</small>
                  <div>{expense?.vendor}</div>
                </div>
              )}
              {expense?.invoiceNumber && (
                <div className="col-12 mb-2">
                  <small className="text-muted">Fatura No</small>
                  <div>{expense?.invoiceNumber}</div>
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Onay/Red Notu */}
        <div className="mb-3">
          <label className="form-label">
            {isApproval ? 'Onay Notu (Opsiyonel)' : 'Red Sebebi *'}
          </label>
          <textarea
            className="form-control"
            rows="4"
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            placeholder={
              isApproval
                ? 'Onay ile ilgili not ekleyebilirsiniz...'
                : 'Red sebebini belirtiniz (zorunlu)...'
            }
            required={!isApproval}
          />
          {!isApproval && (
            <small className="text-danger">
              * Red sebebi belirtmek zorunludur
            </small>
          )}
        </div>

        {/* Uyarı Mesajı */}
        <div className={`alert ${isApproval ? 'alert-success' : 'alert-danger'}`}>
          <i className={`bi ${isApproval ? 'bi-check-circle' : 'bi-x-circle'} me-2`}></i>
          {isApproval ? (
            <span>
              Bu gideri <strong>onaylamak</strong> üzeresiniz. Bu işlem geri alınamaz.
            </span>
          ) : (
            <span>
              Bu gideri <strong>reddetmek</strong> üzeresiniz. Bu işlem geri alınamaz.
            </span>
          )}
        </div>

        {/* Butonlar */}
        <div className="d-flex justify-content-end gap-2">
          <button
            type="button"
            className="btn btn-secondary"
            onClick={handleClose}
            disabled={loading}
          >
            İptal
          </button>
          <button
            type="submit"
            className={`btn ${isApproval ? 'btn-success' : 'btn-danger'}`}
            disabled={loading}
          >
            {loading ? (
              <>
                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                İşleniyor...
              </>
            ) : (
              <>
                <i className={`bi ${isApproval ? 'bi-check-lg' : 'bi-x-lg'} me-2`}></i>
                {isApproval ? 'Onayla' : 'Reddet'}
              </>
            )}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default ApprovalModal;
