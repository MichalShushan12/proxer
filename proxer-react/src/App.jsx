import React, { useState } from 'react';
import { 
  Container, 
  Typography, 
  Box, 
  TextField, 
  Button, 
  FormControl, 
  InputLabel, 
  Select, 
  MenuItem, 
  Paper, 
  CircularProgress,
  Grid
} from '@mui/material';
import AutoFixHighIcon from '@mui/icons-material/AutoFixHigh';
import CodeIcon from '@mui/icons-material/Code';
import axios from 'axios';

function App() {
  // 1. הגדרת State לניהול הטופס והתשובות
  const [originalCode, setOriginalCode] = useState('');
  const [actionType, setActionType] = useState('ShortenCode');
  const [correctedCode, setCorrectedCode] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // 2. פונקציית השליחה לבקאנד ב-C#
  const handleRefactor = async () => {
    if (!originalCode.trim()) {
      setError('בבקשה תדביקי קוד כלשהו לפני הריצה');
      return;
    }

    setLoading(true);
    setError('');
    setCorrectedCode('');

    try {
  const response = await axios.post('https://localhost:7201/api/RefactorLog/process', {
    originalCode: originalCode,
    actionType: actionType,
    targetLanguage: navigator.language || 'he' // שולח את שפת הדפדפן של המשתמש!
  });

  // ה-Controller שלך מחזיר את result ישירות, ובתוכו יש IsSuccess (באות גדולה בגלל ה-C#!)
  if (response.data.isSuccess || response.data.IsSuccess) {
    setCorrectedCode(response.data.correctedCode || response.data.CorrectedCode);
  } else {
    setError(response.data.errorMessage || response.data.ErrorMessage || 'משהו השתבש בתהליך הריפקטור.');
  }

    } catch (err) {
      console.error(err);
      setError('שגיאת תקשורת עם השרת. ודאי שהבקאנד שלך רץ ושה-CORS מוגדר.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4, direction: 'rtl' }}>
      {/* כותרת ראשית */}
      <Box sx={{ textAlign: 'center', mb: 4 }}>
        <Typography variant="h3" component="h1" gutterBottom sx={{ fontWeight: 'bold', color: '#1976d2' }}>
          Proxer AI <AutoFixHighIcon fontSize="large" />
        </Typography>
        <Typography variant="subtitle1" color="textSecondary">
          הכלי החכם לשיפור, קיצור ותיקון קוד C# בזמן אמת
        </Typography>
      </Box>

      {/* אזור הבחירה של הפעולה */}
      <Paper elevation={3} sx={{ p: 3, mb: 4 }}>
        <Grid container spacing={3} alignItems="center">
          <Grid item xs={12} sm={8}>
            <FormControl fullWidth>
              <InputLabel id="action-select-label">מה תרצי לעשות עם הקוד?</InputLabel>
              <Select
                labelId="action-select-label"
                value={actionType}
                label="מה תרצי לעשות עם הקוד?"
                onChange={(e) => setActionType(e.target.value)}
              >
                <MenuItem value="ShortenCode">קצר לי את הקוד (Shorten)</MenuItem>
                <MenuItem value="OptimizePerformance">שפר ביצועים (Optimize)</MenuItem>
                <MenuItem value="AddComments">הוסף הערות הסבר (Document)</MenuItem>
                <MenuItem value="FixSyntax">תקן שגיאות סינטקס (Fix Bugs)</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} sm={4}>
            <Button
              fullWidth
              variant="contained"
              size="large"
              color="primary"
              onClick={handleRefactor}
              disabled={loading}
              startIcon={loading ? <CircularProgress size={24} color="inherit" /> : <CodeIcon />}
              sx={{ height: '56px' }}
            >
              {loading ? 'מריץ ריפקטור...' : 'בצע ריפקטור!'}
            </Button>
          </Grid>
        </Grid>
      </Paper>

      {/* שגיאות במידה ויש */}
      {error && (
        <Box sx={{ mb: 3, p: 2, bgcolor: '#ffebee', color: '#c62828', borderRadius: 1, fontWeight: 'bold' }}>
          {error}
        </Box>
      )}

      {/* מסך מפוצל: קוד מקור מול קוד מתוקן */}
      <Grid container spacing={4}>
        {/* קוד מקור */}
        <Grid item xs={12} md={6}>
          <Typography variant="h6" gutterBottom sx={{ fontWeight: 'bold' }}>
            הקוד המקורי שלך:
          </Typography>
          <TextField
            fullWidth
            multiline
            rows={15}
            variant="outlined"
            placeholder="הדביקי כאן את קוד ה-C# המלוכלך או הארוך..."
            value={originalCode}
            onChange={(e) => setOriginalCode(e.target.value)}
            inputProps={{ style: { fontFamily: 'Consolas, Monaco, monospace', direction: 'ltr' } }}
          />
        </Grid>

        {/* קוד מתוקן מה-AI */}
        <Grid item xs={12} md={6}>
          <Typography variant="h6" gutterBottom sx={{ fontWeight: 'bold', color: '#2e7d32' }}>
            הקוד המשופר מה-AI:
          </Typography>
          <TextField
            fullWidth
            multiline
            rows={15}
            variant="outlined"
            disabled
            value={correctedCode}
            placeholder="התוצאה הנקייה תופיע כאן בהרצה..."
            sx={{ 
              backgroundColor: '#f5f5f5',
              '& .MuiOutlinedInput-root.Mui-disabled .MuiOutlinedInput-input': {
                WebkitTextFillColor: '#1b5e20', // צבע טקסט ירוק כהה וברור לקוד הנקי
              }
            }}
            inputProps={{ style: { fontFamily: 'Consolas, Monaco, monospace', direction: 'ltr' } }}
          />
        </Grid>
      </Grid>
    </Container>
  );
}

export default App;